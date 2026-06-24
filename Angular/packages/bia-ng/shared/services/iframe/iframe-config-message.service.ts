import { Injectable } from '@angular/core';
import { BiaTranslationService } from '@bia-team/bia-ng/core';
import { IframeConfig } from '@bia-team/bia-ng/models';
import { BiaThemeService } from '../../components/layout/services/bia-theme.service';
import { BiaLayoutService } from '../../components/layout/services/layout.service';
import { IframeCommunicationService } from './iframe-communication.service';

@Injectable({
  providedIn: 'root',
})
export class IframeConfigMessageService {
  constructor(
    protected readonly iframeCommunicationService: IframeCommunicationService,
    protected readonly layoutService: BiaLayoutService,
    protected readonly biaTheme: BiaThemeService,
    protected readonly biaTranslation: BiaTranslationService
  ) {}

  register() {
    this.iframeCommunicationService.registerHandler(
      'CONFIG',
      this.configureApplication.bind(this)
    );
  }

  configureApplication(config: IframeConfig) {
    this.biaTranslation.loadAndChangeLanguage(config.language);
    this.layoutService.config.set(config.layoutConfig);
    this.biaTheme.changeTheme(this.layoutService.config().colorScheme);
  }
}
