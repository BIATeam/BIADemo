import {
  Component,
  effect,
  ElementRef,
  HostListener,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription, tap } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { ExternalSiteConfig } from '../../model/external-site-config';
import { IframeConfig } from '../../model/iframe-config';
import { IframeMessage } from '../../model/iframe-message';
import { LayoutHelperService } from '../../services/layout-helper.service';
import { BiaLayoutService } from '../layout/services/layout.service';

export const initExternalSiteConfig: ExternalSiteConfig = {
  baseUrl: '',
  suffixUrl: '',
};

@Component({
  selector: 'bia-external-site',
  templateUrl: './bia-external-site.component.html',
})
export class BiaExternalSiteComponent implements OnInit, OnDestroy {
  @ViewChild('externalSiteIFrame', { static: true })
  iframe: ElementRef<HTMLObjectElement>;

  @Input() config = initExternalSiteConfig;

  protected url: SafeUrl;
  protected isIframeReady = false;
  protected subscription: Subscription = new Subscription();

  constructor(
    protected readonly activatedRoute: ActivatedRoute,
    protected readonly domSanitizer: DomSanitizer,
    protected readonly layoutService: BiaLayoutService,
    protected readonly biaTranslationService: BiaTranslationService,
    protected readonly authService: AuthService
  ) {
    effect(() => {
      this.layoutService.config();
      this.sendConfigToIframe();
    });

    this.subscription.add(
      this.biaTranslationService.currentCulture$
        .pipe(
          tap(() => {
            this.sendConfigToIframe();
          })
        )
        .subscribe()
    );
  }

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.config = snapshot.data['config'];
    const { baseUrl, suffixUrl } = this.config as ExternalSiteConfig;

    this.url = this.domSanitizer.bypassSecurityTrustResourceUrl(
      `${baseUrl}/${suffixUrl}`
    );

    this.sendConfigToIframe();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  @HostListener('window:message', ['$event'])
  receiveMessage(event: MessageEvent<IframeMessage>) {
    if (
      event.origin === this.config.baseUrl &&
      event.data?.type === 'IFRAME_READY'
    ) {
      this.isIframeReady = true;
      this.sendConfigToIframe();
    }
  }

  protected sendConfigToIframe() {
    if (this.iframe?.nativeElement?.contentWindow && this.isIframeReady) {
      const config: IframeConfig = {
        type: 'CONFIG',
        layoutConfig: this.layoutService.config(),
        language: this.biaTranslationService.currentCultureValue,
        loginParams: this.authService.getLoginParameters(),
      };
      this.iframe.nativeElement.contentWindow?.postMessage(
        config,
        this.config.baseUrl
      );
    }
  }

  getIFrameHeight(): string {
    return `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService, '+ 3.5rem')})`;
  }
}
