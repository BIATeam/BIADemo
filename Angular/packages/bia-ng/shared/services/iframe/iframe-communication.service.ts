import { Injectable } from '@angular/core';
import { AppSettingsService } from 'packages/bia-ng/core/public-api';
import { IframeMessage } from 'packages/bia-ng/models/public-api';
import { BiaLayoutService } from '../../components/layout/services/layout.service';

type MessageProcessor<T extends IframeMessage> = (message: T) => void;

interface MessageHandler<T extends IframeMessage> {
  typeGuard: (message: IframeMessage) => message is T;
  processor: MessageProcessor<T>;
}

@Injectable({
  providedIn: 'root',
})
export class IframeCommunicationService {
  protected handlers = new Map<string, MessageHandler<IframeMessage>>();

  constructor(
    protected readonly appSettingsService: AppSettingsService,
    protected readonly layoutService: BiaLayoutService
  ) {}

  readMessage(message: MessageEvent<IframeMessage>) {
    if (
      !this.appSettingsService.appSettings?.iframeConfiguration?.allowedIframeHosts?.find(
        allowedHost => allowedHost.url === message.origin
      ) ||
      !message.data
    ) {
      return;
    }

    this.processMessage(message.data);
  }

  initLayoutInsideIframe() {
    if (!this.appSettingsService.appSettings.iframeConfiguration?.keepLayout) {
      this.layoutService.state.isInIframe = true;
      this.layoutService.hideBreadcrumb();
      this.layoutService.hideFooter();
    }
    this.getConfigFromParent();
  }

  registerHandler<T extends IframeMessage>(
    type: T['type'],
    processor: MessageProcessor<T>
  ) {
    const typeGuard = (message: IframeMessage): message is T => {
      return message.type === type;
    };

    this.handlers.set(type, {
      typeGuard,
      processor: processor as MessageProcessor<IframeMessage>,
    });
  }

  processMessage(message: IframeMessage) {
    const handler = this.handlers.get(message.type);
    if (handler && handler.typeGuard(message)) {
      handler.processor(message);
    } else {
      console.log(
        'Unknown message type or no handler registered:',
        message.type
      );
    }
  }

  protected getConfigFromParent() {
    window.parent.postMessage(
      { type: 'IFRAME_READY' },
      location.ancestorOrigins[0]
    );
  }
}
