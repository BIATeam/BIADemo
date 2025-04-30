import { Injectable } from '@angular/core';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import {
  AppConfig,
  BiaLayoutService,
} from '../components/layout/services/layout.service';
import { IframeConfig } from '../model/iframe-config';
import { IframeMessage } from '../model/iframe-message';

@Injectable({
  providedIn: 'root',
})
export class IframeCommunicationService {
  initializationData: AppConfig;

  constructor(
    protected readonly layoutService: BiaLayoutService,
    protected readonly biaTheme: BiaThemeService,
    protected readonly biaTranslation: BiaTranslationService,
    protected readonly appSettingsService: AppSettingsService
  ) {}

  readMessage(message: MessageEvent<IframeMessage>) {
    if (
      !this.layoutService ||
      !this.appSettingsService.appSettings?.allowedIframeHosts?.find(
        allowedHost => allowedHost.url === message.origin
      ) ||
      !message.data
    ) {
      return;
    }

    const messageData = message.data;
    switch (messageData.type) {
      case 'CONFIG':
        this.configureApplication(messageData as IframeConfig);
    }
  }

  configureApplication(config: IframeConfig) {
    this.biaTranslation.loadAndChangeLanguage(config.language);
    this.layoutService.config.set(config.layoutConfig);
    this.biaTheme.changeTheme(this.layoutService.config().colorScheme);
  }
}
