import {
  AfterViewInit,
  Component,
  ComponentRef,
  HostBinding,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { BiaInjectorService } from 'biang/core';
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

  constructor(
    public activatedRoute: ActivatedRoute,
    protected serviceInjector: BiaInjectorService
  ) {}
  protected dynamicComponent: ComponentRef<any>;

  popupTitle: string;
  style: any;
  maximizable: boolean;
  @HostBinding('class') classes = 'bia-flex';

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;

    this.popupTitle = snapshot.data['title'];
    this.style = { minWidth: '50vw' };
    if (snapshot.data['style']) {
      this.style = snapshot.data['style'];
    }

    this.maximizable = snapshot.data['maximizable'] ?? true;
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
}
