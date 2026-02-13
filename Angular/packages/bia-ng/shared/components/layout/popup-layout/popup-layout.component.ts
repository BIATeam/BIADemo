import {
  AfterViewInit,
  Component,
  ComponentRef,
  HostBinding,
  inject,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import {
  BiaAppConstantsService,
  BiaInjectorService,
} from 'packages/bia-ng/core/public-api';
import { SharedModule } from 'primeng/api';
import { Dialog } from 'primeng/dialog';

@Component({
  selector: 'bia-popup-layout',
  templateUrl: './popup-layout.component.html',
  imports: [Dialog, SharedModule, RouterOutlet, TranslateModule],
})
export class PopupLayoutComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('dynamic', {
    read: ViewContainerRef,
  })
  viewContainerRef: ViewContainerRef;

  private router = inject(Router);

  constructor(
    public activatedRoute: ActivatedRoute,
    protected serviceInjector: BiaInjectorService
  ) {}
  protected dynamicComponent: ComponentRef<any>;

  popupTitle: string;
  popupVisible: boolean = true;
  popupClosable: boolean;
  hasParentUrlSegment: boolean;
  style: any;
  maximizable: boolean;
  @HostBinding('class') classes = 'bia-flex';

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;

    this.popupTitle = snapshot.data['title'];
    this.style = { minWidth: BiaAppConstantsService.defaultPopupMinWidth };
    if (snapshot.data['style']) {
      this.style = snapshot.data['style'];
    }

    this.maximizable = snapshot.data['maximizable'] ?? true;
    this.popupClosable = snapshot.data['closable'] ?? false;
    this.hasParentUrlSegment = snapshot.data['hasParentUrlSegment'] ?? false;
  }
  ngAfterViewInit() {
    setTimeout(() => {
      this.dynamicComponent = this.serviceInjector.addDynamicComponent(
        this.viewContainerRef,
        this.activatedRoute.snapshot.data['injectComponent']
      );
    }, 0);
  }

  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
  }

  onPopupHide() {
    this.router.navigate(['..'], {
      relativeTo: this.hasParentUrlSegment
        ? this.activatedRoute.parent
        : this.activatedRoute,
    });
  }
}
