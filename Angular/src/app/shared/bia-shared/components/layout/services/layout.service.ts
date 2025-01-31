import {
  ComponentPortal,
  ComponentType,
  Portal,
  TemplatePortal,
} from '@angular/cdk/portal';
import {
  Injectable,
  InjectionToken,
  Injector,
  TemplateRef,
  ViewContainerRef,
  effect,
  signal,
} from '@angular/core';
import { MenuItem } from 'primeng/api';
import { BehaviorSubject, Subject, debounceTime } from 'rxjs';
import { STORAGE_THEME_KEY } from 'src/app/core/bia-core/services/bia-theme.service';
import { STORAGE_CULTURE_KEY } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaNavigation } from 'src/app/shared/bia-shared/model/bia-navigation';
import { ConfigDisplay } from '../../../model/config-display';

export const BIA_USER_CONFIG = 'bia-user-config';

export const BIA_LAYOUT_DATA = new InjectionToken<any>('BiaLayoutData');

export type MenuMode =
  | 'static'
  | 'overlay'
  | 'horizontal'
  | 'slim'
  | 'slim-plus'
  | 'reveal'
  | 'drawer';

export type FooterMode = 'bottom' | 'overlay';

export type ColorScheme = 'light' | 'dark';

export interface AppConfig {
  classicStyle: boolean;
  colorScheme: ColorScheme;
  menuMode: MenuMode;
  scale: number;
  showAvatar: boolean;
  footerMode: FooterMode;
}

interface LayoutState {
  staticMenuMobileActive: boolean;
  overlayMenuActive: boolean;
  overlayFooterActive: boolean;
  staticMenuDesktopInactive: boolean;
  configSidebarVisible: boolean;
  menuHoverActive: boolean;
  topbarMenuActive: boolean;
  menuProfileActive: boolean;
  sidebarActive: boolean;
  anchored: boolean;
  fullscreen: boolean;
  isSmallScreen: boolean;
}

const DEFAULT_LAYOUT_CONFIG: AppConfig = {
  classicStyle: false,
  colorScheme: 'light',
  menuMode: 'static',
  scale: 14,
  showAvatar: true,
  footerMode: 'overlay',
};

const DEFAULT_CONFIG_DISPLAY: ConfigDisplay = {
  showEditAvatar: true,
  showLang: true,
  showScale: true,
  showTheme: true,
  showMenuStyle: false,
  showFooterStyle: false,
  showToggleStyle: false,
};

@Injectable({
  providedIn: 'root',
})
export class BiaLayoutService {
  _config: AppConfig = DEFAULT_LAYOUT_CONFIG;
  _configDisplay: ConfigDisplay = DEFAULT_CONFIG_DISPLAY;

  state: LayoutState = {
    staticMenuDesktopInactive: false,
    overlayMenuActive: false,
    overlayFooterActive: false,
    configSidebarVisible: false,
    staticMenuMobileActive: false,
    topbarMenuActive: false,
    menuHoverActive: false,
    sidebarActive: false,
    anchored: false,
    fullscreen: false,
    menuProfileActive: false,
    isSmallScreen: false,
  };

  config = signal<AppConfig>(this._config);
  configDisplay = signal<ConfigDisplay>(this._configDisplay);

  private configUpdate = new BehaviorSubject<AppConfig>(this._config);
  private configDisplayUpdate = new BehaviorSubject<ConfigDisplay>(
    this._configDisplay
  );
  private overlayOpen = new Subject<any>();
  private overlayFooterOpen = new Subject<any>();
  private topbarMenuOpen = new Subject<any>();
  private menuProfileOpen = new Subject<any>();

  protected footerPortal = new BehaviorSubject<Portal<any> | null>(null);
  protected mainBarPortal = new BehaviorSubject<Portal<any> | null>(null);
  protected mainBarHidden = new BehaviorSubject<boolean>(false);
  protected footerHidden = new BehaviorSubject<boolean>(false);
  protected breadcrumbHidden = new BehaviorSubject<boolean>(false);
  protected breadcrumbRefresh = new BehaviorSubject<boolean>(false);

  configUpdate$ = this.configUpdate.asObservable();
  configDisplayUpdate$ = this.configDisplayUpdate.asObservable();
  overlayOpen$ = this.overlayOpen.asObservable();
  overlayFooterOpen$ = this.overlayFooterOpen.asObservable();
  topbarMenuOpen$ = this.topbarMenuOpen.asObservable();
  menuProfileOpen$ = this.menuProfileOpen.asObservable();

  // Whether user should choose a single role
  footerPortal$ = this.footerPortal.asObservable();
  mainBarPortal$ = this.mainBarPortal.asObservable();
  mainBarHidden$ = this.mainBarHidden.asObservable().pipe(debounceTime(0));
  footerHidden$ = this.footerHidden.asObservable().pipe(debounceTime(0));
  breadcrumbHidden$ = this.breadcrumbHidden
    .asObservable()
    .pipe(debounceTime(0));
  breadcrumbRefresh$ = this.breadcrumbRefresh.asObservable();

  constructor() {
    effect(() => {
      const config = this.config();
      this.changeScale(config.scale);
      this.onConfigUpdate();
      localStorage.setItem(BIA_USER_CONFIG, JSON.stringify(config));
    });
    this.checkSmallScreen();
  }

  changeTheme() {
    const { colorScheme } = this.config();
    const themeLink = <HTMLLinkElement>document.getElementById('theme-link');
    const themeLinkHref = themeLink.getAttribute('href');
    if (themeLinkHref) {
      const newHref = themeLinkHref
        .split('/')
        .map(el =>
          el == `theme-${this._config.colorScheme}`
            ? (el = `theme-${colorScheme}`)
            : el
        )
        .join('/');
      this.replaceThemeLink(newHref);
    }
  }

  replaceThemeLink(href: string) {
    const id = 'theme-link';
    const themeLink = <HTMLLinkElement>document.getElementById(id);
    const cloneLinkElement = <HTMLLinkElement>themeLink.cloneNode(true);

    cloneLinkElement.setAttribute('href', href);
    cloneLinkElement.setAttribute('id', id + '-clone');

    themeLink.parentNode?.insertBefore(cloneLinkElement, themeLink.nextSibling);
    cloneLinkElement.addEventListener('load', () => {
      themeLink.remove();
      cloneLinkElement.setAttribute('id', id);
    });
  }

  changeScale(value: number) {
    document.documentElement.style.fontSize = `${value}px`;
  }

  onMenuToggle() {
    if (this.isOverlay()) {
      this.state.overlayMenuActive = !this.state.overlayMenuActive;

      if (this.state.overlayMenuActive) {
        this.overlayOpen.next(null);
      }
    }

    if (this.isDesktop()) {
      this.state.staticMenuDesktopInactive =
        !this.state.staticMenuDesktopInactive;
    } else {
      this.state.staticMenuMobileActive = !this.state.staticMenuMobileActive;

      if (this.state.staticMenuMobileActive) {
        this.overlayOpen.next(null);
      }
    }
  }

  onFooterToggle() {
    if (this.isFooterOverlay()) {
      this.state.overlayFooterActive = !this.state.overlayFooterActive;

      if (this.state.overlayFooterActive) {
        this.overlayFooterOpen.next(null);
      }
    }
  }

  onTopbarMenuToggle() {
    this.state.topbarMenuActive = !this.state.topbarMenuActive;
    if (this.state.topbarMenuActive) {
      this.topbarMenuOpen.next(null);
    }
  }

  menuOpen() {
    if (
      (this.isOverlay() && !this.state.overlayMenuActive) ||
      (this.isDesktop() && this.state.staticMenuDesktopInactive) ||
      (!this.isDesktop() && !this.state.staticMenuMobileActive)
    ) {
      this.onMenuToggle();
    }
  }

  menuClose() {
    if (
      (this.isOverlay() && this.state.overlayMenuActive) ||
      (this.isDesktop() && !this.state.staticMenuDesktopInactive) ||
      (!this.isDesktop() && this.state.staticMenuMobileActive)
    ) {
      this.onMenuToggle();
    }
  }

  onOverlaySubmenuOpen() {
    this.overlayOpen.next(null);
  }

  isOverlay() {
    return this.config().menuMode === 'overlay';
  }

  isFooterOverlay() {
    return this.config().footerMode === 'overlay';
  }

  isDesktop() {
    return window.innerWidth > 991;
  }

  isSlim() {
    return this.config().menuMode === 'slim';
  }

  isSlimPlus() {
    return this.config().menuMode === 'slim-plus';
  }

  isDrawer() {
    return this.config().menuMode === 'drawer';
  }

  isHorizontal() {
    return this.config().menuMode === 'horizontal';
  }

  isMobile() {
    return !this.isDesktop();
  }

  onConfigUpdate() {
    this._config = { ...this.config() };
    this.configUpdate.next(this.config());
  }

  defaultConfigUpdate(
    config: Partial<AppConfig>,
    overwriteLocalStorageConfig = false
  ) {
    const lValue = localStorage.getItem(BIA_USER_CONFIG);
    let valueToUpdate: Partial<AppConfig>;
    if (!overwriteLocalStorageConfig && lValue) {
      valueToUpdate = JSON.parse(lValue);
    } else {
      valueToUpdate = config;
    }
    this.config.update(currentValue => {
      return { ...currentValue, ...valueToUpdate };
    });

    this.onConfigUpdate();
  }

  setConfigDisplay(configDisplay: Partial<ConfigDisplay>) {
    this.configDisplay.update(currentValue => {
      return { ...currentValue, ...configDisplay };
    });
    this._configDisplay = { ...this.configDisplay() };
    this.configDisplayUpdate.next(this.configDisplay());
  }

  mapNavigationToMenuItems(
    navigationItems: BiaNavigation[],
    withIcons = false
  ): MenuItem[] {
    const navMenuItems: MenuItem[] = [];
    navigationItems.forEach(menu => {
      const childrenMenuItem: MenuItem[] = [];
      if (menu.children) {
        childrenMenuItem.push(
          ...this.mapNavigationToMenuItems(menu.children, withIcons)
        );
      }
      navMenuItems.push({
        id: menu.labelKey,
        routerLink: menu.path,
        icon: withIcons ? menu.icon : undefined,
        items: childrenMenuItem.length > 0 ? childrenMenuItem : undefined,
      });
    });
    return navMenuItems;
  }

  processMenuTranslation(children: MenuItem[], translations: any) {
    for (const item of children) {
      if (item.separator) continue;
      item.label = item.id == undefined ? '---' : translations[item.id];
      if (item.items) {
        this.processMenuTranslation(item.items, translations);
      }
    }
  }

  changeFooter<T, D>(
    componentOrTemplateRef: ComponentType<T> | TemplateRef<T> | null,
    injector?: Injector,
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
      data
    );
  }

  changeMainBar<T, D>(
    componentOrTemplateRef: ComponentType<T> | TemplateRef<T> | null,
    injector?: Injector,
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

  setFullscreen(fullscreenMode: boolean) {
    this.state.fullscreen = fullscreenMode;
  }

  protected setPortal<T, D>(
    portalSubject: BehaviorSubject<Portal<any> | null>,
    componentOrTemplateRef: ComponentType<T> | TemplateRef<T>,
    injector?: Injector,
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
      portal = new ComponentPortal(componentOrTemplateRef, null, finalInjector);
    }
    portalSubject.next(portal);
    return portal;
  }

  onMenuProfileToggle() {
    this.state.menuProfileActive = !this.state.menuProfileActive;
    if (
      this.state.menuProfileActive &&
      this.isHorizontal() &&
      this.isDesktop()
    ) {
      this.menuProfileOpen.next(null);
    }
  }

  toggleStyle() {
    this.config.update(config => ({
      ...config,
      classicStyle: !this._config.classicStyle,
      scale: this._config.classicStyle ? 14 : 16,
    }));
  }

  clearSession() {
    const culture = localStorage.getItem(STORAGE_CULTURE_KEY);
    const theme = localStorage.getItem(STORAGE_THEME_KEY);
    const config = localStorage.getItem(BIA_USER_CONFIG);
    localStorage.clear();
    if (culture !== null) localStorage.setItem(STORAGE_CULTURE_KEY, culture);
    if (theme !== null) localStorage.setItem(STORAGE_THEME_KEY, theme);
    if (config !== null) localStorage.setItem(BIA_USER_CONFIG, config);
    sessionStorage.clear();
  }

  checkSmallScreen() {
    this.state.isSmallScreen = window.matchMedia('(max-width:991px)').matches;
  }
}
