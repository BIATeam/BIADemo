import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { BiaLayoutService } from './shared/bia-shared/components/layout/services/layout.service';
import { LayoutHelperService } from './shared/bia-shared/services/layout-helper.service';

export interface WrapperConfig {
  url: string;
}

export const initWrapperConfig: WrapperConfig = {
  url: '',
};

@Component({
  selector: 'app-external-site',
  templateUrl: './external-site.component.html',
})
export class ExternalSiteComponent implements OnInit {
  @ViewChild('wrapperContainer') wrapperContainer: ElementRef;
  url: SafeUrl;

  @Input() config = initWrapperConfig;

  constructor(
    protected readonly activatedRoute: ActivatedRoute,
    protected readonly domSanitizer: DomSanitizer,
    protected readonly layoutService: BiaLayoutService
  ) {}

  ngOnInit() {
    this.layoutService.hideBreadcrumb();

    const snapshot = this.activatedRoute.snapshot;
    this.config = snapshot.data['config'];
    const { url } = this.config;

    this.url = this.domSanitizer.bypassSecurityTrustResourceUrl(url);
  }

  getIFrameHeight(): string {
    return `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService, '+ 3.5rem')})`;
  }
}
