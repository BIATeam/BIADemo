import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  BiaInjectExternalService,
  BiaMatomoService,
} from '@bia-team/bia-ng/core';
import { IframeMessage } from '@bia-team/bia-ng/models';
import {
  BiaLayoutService,
  IframeCommunicationService,
  IframeConfigMessageService,
} from '@bia-team/bia-ng/shared';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNG } from 'primeng/config';
import { Subject, fromEvent } from 'rxjs';
import { auditTime, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styles: [
    ':host { min-height: 100vh; display: flex; flex-direction: column; }',
  ],
  imports: [RouterOutlet],
})
export class AppComponent implements OnInit, OnDestroy {
  isInIframe: boolean;
  private destroy$ = new Subject<void>();

  constructor(
    private biaMatomoService: BiaMatomoService,
    private biaExternalJsService: BiaInjectExternalService,
    private primeNgConfig: PrimeNG,
    private translateService: TranslateService,
    private layoutService: BiaLayoutService,
    private iframeCommunicationService: IframeCommunicationService,
    private iframeConfigMessageService: IframeConfigMessageService,
    private ngZone: NgZone
  ) {
    this.layoutService.defaultConfigUpdate({});
    this.layoutService.setConfigDisplay({
      // Begin BIADemo
      showMenuStyle: [
        'static',
        'overlay',
        'horizontal',
        'slim',
        'slim-plus',
        'reveal',
        'drawer',
      ],
      showFooterStyle: true,
      showMenuProfilePosition: true,
      // End BIADemo
    });
  }

  ngOnInit() {
    this.isInIframe = window !== window.top;
    if (this.isInIframe) {
      this.iframeConfigMessageService.register();
      this.iframeCommunicationService.initLayoutInsideIframe();
    }
    this.biaMatomoService.init();
    this.biaExternalJsService.init();
    this.translateService
      .get('primeng')
      .subscribe(res => this.primeNgConfig.setTranslation(res));
    this.registerWindowListeners();
    this.checkSmallScreen();
  }

  receiveMessage(event: MessageEvent<IframeMessage>) {
    if (this.iframeCommunicationService) {
      this.iframeCommunicationService.readMessage(event);
    }
  }

  checkSmallScreen() {
    this.layoutService.checkSmallScreen();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private registerWindowListeners() {
    this.ngZone.runOutsideAngular(() => {
      if (this.isInIframe) {
        fromEvent<MessageEvent<IframeMessage>>(window, 'message')
          .pipe(takeUntil(this.destroy$))
          .subscribe(event =>
            this.ngZone.run(() => this.receiveMessage(event))
          );
      }

      fromEvent<Event>(window, 'resize')
        .pipe(auditTime(150), takeUntil(this.destroy$))
        .subscribe(() => this.ngZone.run(() => this.checkSmallScreen()));
    });
  }
}
