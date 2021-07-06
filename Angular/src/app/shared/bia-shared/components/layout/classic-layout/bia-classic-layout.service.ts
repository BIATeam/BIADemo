import { ComponentPortal, ComponentType, Portal, TemplatePortal, PortalInjector } from '@angular/cdk/portal';
import {
  Injectable,
  Injector,
  TemplateRef,
  ComponentFactoryResolver,
  InjectionToken,
  ViewContainerRef
} from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

export const BIA_LAYOUT_DATA = new InjectionToken<any>('BiaLayoutData');

@Injectable()
export class BiaClassicLayoutService {
  private footerPortal = new BehaviorSubject<Portal<any> | null>(null);
  private mainBarPortal = new BehaviorSubject<Portal<any> | null>(null);
  private mainBarHidden = new BehaviorSubject<boolean>(false);
  private footerHidden = new BehaviorSubject<boolean>(false);
  private breadcrumbHidden = new BehaviorSubject<boolean>(false);
  private breadcrumbRefresh = new BehaviorSubject<boolean>(false);

  // Whether user should choose a single role
  private singleRoleMode = new BehaviorSubject<boolean>(environment.singleRoleMode ?? true);

  footerPortal$ = this.footerPortal.asObservable();
  mainBarPortal$ = this.mainBarPortal.asObservable();
  mainBarHidden$ = this.mainBarHidden.asObservable();
  footerHidden$ = this.footerHidden.asObservable();
  breadcrumbHidden$ = this.breadcrumbHidden.asObservable();
  breadcrumbRefresh$ = this.breadcrumbRefresh.asObservable();
  singleRoleMode$ = this.singleRoleMode.asObservable();

  changeFooter<T, D>(
    componentOrTemplateRef: ComponentType<T> | TemplateRef<T> | null,
    injector?: Injector,
    componentFactoryResolver?: ComponentFactoryResolver,
    data?: D
  ) {
    if (!componentOrTemplateRef) {
      this.footerPortal.next(null);
      return;
    }
    return this.setPortal(this.footerPortal, componentOrTemplateRef, injector, componentFactoryResolver, data);
  }

  changeMainBar<T, D>(
    componentOrTemplateRef: ComponentType<T> | TemplateRef<T> | null,
    injector?: Injector,
    componentFactoryResolver?: ComponentFactoryResolver,
    data?: D
  ) {
    if (!componentOrTemplateRef) {
      this.mainBarPortal.next(null);
      return;
    }
    return this.setPortal(this.mainBarPortal, componentOrTemplateRef, injector, componentFactoryResolver, data);
  }

  hideMainBar() {
    this.mainBarHidden.next(true);
  }

  showMainBar() {
    this.mainBarHidden.next(false);
  }

  hideFooter() {
    this.footerHidden.next(true);
  }

  showFooter() {
    this.footerHidden.next(false);
  }

  hideBreadcrumb() {
    this.breadcrumbHidden.next(true);
  }

  showBreadcrumb() {
    this.breadcrumbHidden.next(false);
  }

  refreshBreadcrumb() {
    this.breadcrumbRefresh.next(!this.breadcrumbRefresh.value);
  }

  private setPortal<T, D>(
    portalSubject: BehaviorSubject<Portal<any> | null>,
    componentOrTemplateRef: ComponentType<T> | TemplateRef<T>,
    injector?: Injector,
    componentFactoryResolver?: ComponentFactoryResolver,
    data?: D
  ) {
    let portal;
    if (componentOrTemplateRef instanceof TemplateRef) {
      portal = new TemplatePortal<T>(componentOrTemplateRef, <ViewContainerRef>{});
    } else {
      let finalInjector = injector;
      if (data) {
        const injectionTokens = new WeakMap<any, any>([[BIA_LAYOUT_DATA, data]]);
        if (injector !== undefined) {
          finalInjector = new PortalInjector(injector, injectionTokens);
        }
      }
      portal = new ComponentPortal(componentOrTemplateRef, null, finalInjector, componentFactoryResolver);
    }
    portalSubject.next(portal);
    return portal;
  }
}
