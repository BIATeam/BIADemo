import {
  Component,
  effect,
  ElementRef,
  HostListener,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { ExternalSiteConfig } from '../../model/external-site-config';
import { IframeConfig } from '../../model/iframe-config';
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
export class BiaExternalSiteComponent implements OnInit {
  @ViewChild('externalSiteIFrame', { static: true })
  iframe: ElementRef<HTMLObjectElement>;

  url: SafeUrl;

  @Input() config = initExternalSiteConfig;

  constructor(
    protected readonly activatedRoute: ActivatedRoute,
    protected readonly domSanitizer: DomSanitizer,
    protected readonly layoutService: BiaLayoutService,
    protected readonly biaTranslationService: BiaTranslationService,
    protected readonly authService: AuthService
  ) {
    effect(() => {
      this.sendConfigToIframe();
    });

    this.biaTranslationService.currentCulture$
      .pipe(
        tap(() => {
          this.sendConfigToIframe();
        })
      )
      .subscribe();
  }

  private sendConfigToIframe() {
    if (this.iframe?.nativeElement?.contentWindow) {
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

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.config = snapshot.data['config'];
    const { baseUrl, suffixUrl } = this.config as ExternalSiteConfig;

    this.url = this.domSanitizer.bypassSecurityTrustResourceUrl(
      `${baseUrl}/${suffixUrl}`
    );

    this.sendConfigToIframe();
  }

  @HostListener('window:message', ['$event'])
  receiveMessage(event: MessageEvent<string>) {
    if (event.origin === this.config.baseUrl && event.data === 'iframeReady') {
      this.sendConfigToIframe();
    }
  }

  getIFrameHeight(): string {
    return `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService, '+ 3.5rem')})`;
  }
}
