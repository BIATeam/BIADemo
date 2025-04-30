import { Injectable } from '@angular/core';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { BiaLayoutService } from '../components/layout/services/layout.service';
import { IframeConfig } from '../model/iframe-config';
import { IframeMessage } from '../model/iframe-message';

type MessageProcessor<T extends IframeMessage> = (message: T) => void;

interface MessageHandler<T extends IframeMessage> {
  typeGuard: (message: IframeMessage) => message is T;
  processor: MessageProcessor<T>;
}

@Injectable({
  providedIn: 'root',
})
export class IframeCommunicationService {
  private handlers = new Map<string, MessageHandler<IframeMessage>>();

  constructor(
    protected readonly layoutService: BiaLayoutService,
    protected readonly biaTheme: BiaThemeService,
    protected readonly biaTranslation: BiaTranslationService,
    protected readonly appSettingsService: AppSettingsService
  ) {
    this.registerHandler('CONFIG', this.configureApplication.bind(this));
  }

  readMessage(message: MessageEvent<IframeMessage>) {
    if (
      !this.appSettingsService.appSettings?.allowedIframeHosts?.find(
        allowedHost => allowedHost.url === message.origin
      ) ||
      !message.data
    ) {
      return;
    }

    this.processMessage(message.data);
  }

  configureApplication(config: IframeConfig) {
    this.biaTranslation.loadAndChangeLanguage(config.language);
    this.layoutService.config.set(config.layoutConfig);
    this.biaTheme.changeTheme(this.layoutService.config().colorScheme);
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
}
