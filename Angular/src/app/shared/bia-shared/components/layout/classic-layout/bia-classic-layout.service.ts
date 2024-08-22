/* eslint-disable deprecation/deprecation */
import {
  ComponentPortal,
  ComponentType,
  Portal,
  TemplatePortal,
} from '@angular/cdk/portal';
import {
  Injectable,
  Injector,
  TemplateRef,
  ComponentFactoryResolver,
  InjectionToken,
  ViewContainerRef,
} from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export const BIA_LAYOUT_DATA = new InjectionToken<any>('BiaLayoutData');

@Injectable()
export class BiaClassicLayoutService {
  protected footerPortal = new BehaviorSubject<Portal<any> | null>(null);
  protected mainBarPortal = new BehaviorSubject<Portal<any> | null>(null);
  protected mainBarHidden = new BehaviorSubject<boolean>(false);
  protected footerHidden = new BehaviorSubject<boolean>(false);
  protected breadcrumbHidden = new BehaviorSubject<boolean>(false);
  protected breadcrumbRefresh = new BehaviorSubject<boolean>(false);

  // Whether user should choose a single role
  footerPortal$ = this.footerPortal.asObservable();
  mainBarPortal$ = this.mainBarPortal.asObservable();
  mainBarHidden$ = this.mainBarHidden.asObservable();
  footerHidden$ = this.footerHidden.asObservable();
  breadcrumbHidden$ = this.breadcrumbHidden.asObservable();
  breadcrumbRefresh$ = this.breadcrumbRefresh.asObservable();

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
    return this.setPortal(
      this.footerPortal,
      componentOrTemplateRef,
      injector,
      componentFactoryResolver,
      data
    );
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
    return this.setPortal(
      this.mainBarPortal,
      componentOrTemplateRef,
      injector,
      componentFactoryResolver,
      data
    );
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

  protected setPortal<T, D>(
    portalSubject: BehaviorSubject<Portal<any> | null>,
    componentOrTemplateRef: ComponentType<T> | TemplateRef<T>,
    injector?: Injector,
    componentFactoryResolver?: ComponentFactoryResolver,
    data?: D
  ) {
    let portal;
    if (componentOrTemplateRef instanceof TemplateRef) {
      portal = new TemplatePortal<T>(
        componentOrTemplateRef,
        <ViewContainerRef>{}
      );
    } else {
      let finalInjector = injector;
      if (data) {
        const injectionTokens = new WeakMap<any, any>([
          [BIA_LAYOUT_DATA, data],
        ]);
        if (injector !== undefined) {
          // finalInjector = new PortalInjector(injector, injectionTokens);
          finalInjector = Injector.create({
            parent: injector,
            providers: [{ provide: injectionTokens, useValue: data }],
          });
        }
      }
      portal = new ComponentPortal(
        componentOrTemplateRef,
        null,
        finalInjector,
        componentFactoryResolver
      );
    }
    portalSubject.next(portal);
    return portal;
  }
}
