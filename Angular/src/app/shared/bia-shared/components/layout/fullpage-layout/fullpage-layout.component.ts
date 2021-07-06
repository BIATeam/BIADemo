import { AfterViewInit, ComponentRef, HostBinding, OnInit, ViewContainerRef } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { filter } from 'rxjs/operators';
import { BiaInjectorService } from 'src/app/core/bia-core/services/bia-injector.service';

@Component({
  selector: 'full-page-layout',
  templateUrl: './fullpage-layout.component.html',
})
export class FullPageLayoutComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('dynamic', {
    read: ViewContainerRef
  }) viewContainerRef: ViewContainerRef;

  constructor(
    public activatedRoute: ActivatedRoute,
    private router: Router,
    private serviceInjector: BiaInjectorService
  ) {
  }

  public $displayPageComponent: BehaviorSubject<boolean>;
  public displayPageComponent: boolean;
  protected dynamicComponent: ComponentRef<any>;
  @HostBinding('class.bia-flex') flex = true;

  ngOnInit() {
    this.$displayPageComponent = new BehaviorSubject<boolean>(true);
    this.displayPageComponent = true;
    this.checkVisibility();
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.checkVisibility();
    });
  }

  checkVisibility() {
    const snapshot = this.activatedRoute.snapshot;
    let child: ActivatedRouteSnapshot | null;
    child = snapshot.firstChild;
    let hasFullPageChild = false;
    while (child) {
      if (child.component === FullPageLayoutComponent) {
        hasFullPageChild = true;
        break;
      }
      child = child?.firstChild;
    }
    if (hasFullPageChild === this.$displayPageComponent.value) {
      this.$displayPageComponent.next(!hasFullPageChild);
      this.displayPageComponent = !hasFullPageChild;
    }
  }
  ngAfterViewInit() {
    this.$displayPageComponent.subscribe(() => {
      const snapshot = this.activatedRoute.snapshot;
      if (this.$displayPageComponent.value && this.dynamicComponent === undefined) {
        setTimeout(() => {
          this.dynamicComponent = this.serviceInjector.addDynamicComponent(this.viewContainerRef, snapshot.data['InjectComponent']);
        }, 0);
      } else {
        if (this.dynamicComponent !== undefined) {
          if (this.$displayPageComponent.value) {
            this.dynamicComponent.instance.OnDisplay();
          } else {
            this.dynamicComponent.instance.OnHide();
          }
        }
        // if( this.dynamicComponent !== undefined ) this.dynamicComponent.destroy();
      }
    });
  }
  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
  }

}
