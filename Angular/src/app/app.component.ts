import { Component, HostListener, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BiaInjectExternalService, BiaMatomoService } from 'biang/core';
import { IframeMessage } from 'biang/models';
import {
  BiaLayoutService,
  IframeCommunicationService,
  IframeConfigMessageService,
} from 'biang/shared';
import { PrimeNG } from 'primeng/config';

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

  constructor(
    private biaMatomoService: BiaMatomoService,
    private biaExternalJsService: BiaInjectExternalService,
    private primeNgConfig: PrimeNG,
    private translateService: TranslateService,
    private layoutService: BiaLayoutService,
    private iframeCommunicationService: IframeCommunicationService,
    private iframeConfigMessageService: IframeConfigMessageService
  ) {
    this.layoutService.defaultConfigUpdate({});
    this.layoutService.setConfigDisplay({
      // Begin BIADemo
      showMenuStyle: true,
      showFooterStyle: true,
      showMenuProfilePosition: true,
      // End BIADemo
    });
  }

  ngOnInit() {
    this.isInIframe = window !== window.top;
    if (this.isInIframe) {
      this.iframeConfigMessageService.register();
      window.addEventListener('message', this.receiveMessage, false);
      this.iframeCommunicationService.initLayoutInsideIframe();
    }
    this.biaMatomoService.init();
    this.biaExternalJsService.init();
    this.translateService
      .get('primeng')
      .subscribe(res => this.primeNgConfig.setTranslation(res));
    this.checkSmallScreen();
  }

  @HostListener('window:message', ['$event'])
  receiveMessage(event: MessageEvent<IframeMessage>) {
    if (this.iframeCommunicationService) {
      this.iframeCommunicationService.readMessage(event);
    }
  }

  @HostListener('window:resize', ['$event'])
  checkSmallScreen() {
    this.layoutService.checkSmallScreen();
  }
}
