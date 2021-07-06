import {
  AfterViewInit,
  ComponentRef,
  HostBinding,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewContainerRef
} from '@angular/core';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BiaInjectorService } from 'src/app/core/bia-core/services/bia-injector.service';

@Component({
  selector: 'popup-layout',
  templateUrl: './popup-layout.component.html'
})
export class PopupLayoutComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('dynamic', {
    read: ViewContainerRef
  })
  viewContainerRef: ViewContainerRef;

  constructor(public activatedRoute: ActivatedRoute, private serviceInjector: BiaInjectorService) {}
  protected dynamicComponent: ComponentRef<any>;

  popupTitle: string;
  style: any;
  maximizable: boolean;
  @HostBinding('class.bia-flex') flex = true;

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;

    this.popupTitle = snapshot.data['title'];
    this.style = { minWidth: '50vw' };
    if (snapshot.data['style']) {
      this.style = snapshot.data['style'];
    }
    this.maximizable = true;
    if (snapshot.data['maximizable']) {
      this.maximizable = snapshot.data['maximizable'];
    }
  }
  ngAfterViewInit() {
    setTimeout(() => {
      this.dynamicComponent = this.serviceInjector.addDynamicComponent(
        this.viewContainerRef,
        this.activatedRoute.snapshot.data['InjectComponent']
      );
    }, 0);
  }
  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
  }
}
