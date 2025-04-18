import { Component, HostListener, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNG } from 'primeng/config';
import { BiaInjectExternalService } from './core/bia-core/services/bia-inject-external.service';
import { BiaThemeService } from './core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from './core/bia-core/services/bia-translation.service';
import { BiaMatomoService } from './core/bia-core/services/matomo/bia-matomo.service';
import { AppSettingsService } from './domains/bia-domains/app-settings/services/app-settings.service';
import {
  AppConfig,
  BiaLayoutService,
} from './shared/bia-shared/components/layout/services/layout.service';
import { IframeConfig } from './shared/bia-shared/model/iframe-config';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styles: [
    ':host { min-height: 100vh; display: flex; flex-direction: column; }',
  ],
  imports: [RouterOutlet],
})
export class AppComponent implements OnInit {
  isInIframe: boolean;
  initializationData: AppConfig;

  constructor(
    private biaMatomoService: BiaMatomoService,
    private biaExternalJsService: BiaInjectExternalService,
    private primeNgConfig: PrimeNG,
    private translateService: TranslateService,
    private layoutService: BiaLayoutService,
    private biaTheme: BiaThemeService,
    private biaTranslation: BiaTranslationService,
    private appSettingsService: AppSettingsService
  ) {
    this.layoutService.defaultConfigUpdate({});
    this.layoutService.setConfigDisplay({
      // Begin BIADemo
      showMenuStyle: true,
      showFooterStyle: true,
      showToggleStyle: true,
      showMenuProfilePosition: true,
      // End BIADemo
    });
  }

  ngOnInit() {
    this.isInIframe = window !== window.top;
    if (this.isInIframe) {
      window.addEventListener('message', this.receiveMessage, false);
      this.layoutService.state.isInIframe = this.isInIframe;
      this.layoutService.hideBreadcrumb();
      this.layoutService.hideFooter();
      this.getConfigFromParent();
    }
    this.biaMatomoService.init();
    this.biaExternalJsService.init();
    this.translateService
      .get('primeng')
      .subscribe(res => this.primeNgConfig.setTranslation(res));
    this.checkSmallScreen();
  }

  getConfigFromParent() {
    window.parent.postMessage('iframeReady', location.ancestorOrigins[0]);
  }

  @HostListener('window:message', ['$event'])
  receiveMessage(event: MessageEvent<IframeConfig>) {
    if (
      !this.layoutService ||
      !this.appSettingsService.appSettings?.allowedIframeHosts?.find(
        allowedHost => allowedHost.url === event.origin
      ) ||
      !event.data
    ) {
      return;
    }

    this.initializationData = event.data.layoutConfig;
    this.biaTranslation.loadAndChangeLanguage(event.data.language);
    this.layoutService.config.set(this.initializationData);
    this.biaTheme.changeTheme(this.layoutService.config().colorScheme);
  }

  @HostListener('window:resize', ['$event'])
  checkSmallScreen() {
    this.layoutService.checkSmallScreen();
  }
}
