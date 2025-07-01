import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import {
  Component,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterOutlet,
} from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';
import { Breadcrumb } from 'primeng/breadcrumb';
import { Observable, Subscription, filter, map } from 'rxjs';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { EnvironmentType } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { BiaNavigation } from 'src/app/shared/bia-shared/model/bia-navigation';
import {
  APP_SUPPORTED_TRANSLATIONS,
  ROUTE_DATA_BREADCRUMB,
  ROUTE_DATA_CAN_NAVIGATE,
  ROUTE_DATA_NO_MARGIN,
  ROUTE_DATA_NO_PADDING,
} from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { BiaLayoutService } from '../../services/layout.service';
import { MenuService } from '../../services/menu.service';
import { BiaUltimaConfigComponent } from '../config/ultima-config.component';
import { BiaUltimaFooterComponent } from '../footer/ultima-footer.component';
import { BiaUltimaSidebarComponent } from '../sidebar/ultima-sidebar.component';
import { BiaUltimaTopbarComponent } from '../topbar/ultima-topbar.component';

@Component({
  selector: 'bia-ultima-layout',
  templateUrl: './ultima-layout.component.html',
  styleUrls: ['./ultima-layout.component.scss'],
  imports: [
    NgClass,
    BiaUltimaTopbarComponent,
    BiaUltimaSidebarComponent,
    NgIf,
    Breadcrumb,
    RouterOutlet,
    BiaUltimaFooterComponent,
    AsyncPipe,
    TranslateModule,
    BiaUltimaConfigComponent,
  ],
})
export class BiaUltimaLayoutComponent implements OnInit, OnDestroy {
  @HostBinding('class.no-margin') noMargin = false;
  @HostBinding('class.no-padding') noPadding = false;
  @Input() version: string;
  @Input() appTitle: string;
  @Input() menus: BiaNavigation[];
  @Input() username?: string;
  @Input() login: string;
  @Input() headerLogos: string[];
  @Input() footerLogo = 'assets/bia/img/Footer.png';
  @Input() supportedLangs = APP_SUPPORTED_TRANSLATIONS;
  @Input() allowThemeChange = true;
  @Input() companyName = 'BIA';
  @Input() helpUrl?: string;
  @Input() reportUrl?: string;
  @Input() enableNotifications?: boolean;

  overlayMenuOpenSubscription: Subscription;
  menuOutsideClickListener: any;
  menuScrollListener: any;
  footerOutsideClickListener: any;
  topbarMenuOutsideClickListener: any;
  menuProfileOutsideClickListener: any;

  menuItems: MenuItem[];

  protected sub = new Subscription();

  @ViewChild(BiaUltimaSidebarComponent) appSidebar!: BiaUltimaSidebarComponent;
  @ViewChild(BiaUltimaTopbarComponent) appTopbar!: BiaUltimaTopbarComponent;
  @ViewChild(BiaUltimaFooterComponent) appFooter!: BiaUltimaFooterComponent;

  envName$: Observable<string | undefined>;
  showEnvironmentMessage$: Observable<boolean>;
  cssClassEnv: string;

  constructor(
    protected biaTranslation: BiaTranslationService,
    protected biaTheme: BiaThemeService,
    protected menuService: MenuService,
    public layoutService: BiaLayoutService,
    public renderer: Renderer2,
    protected translateService: TranslateService,
    public router: Router,
    protected activatedRoute: ActivatedRoute,
    protected store: Store<AppState>
  ) {
    this.hideMenuProfile();
    this.overlayMenuSubscription();
    this.overlayFooterSubscription();
    this.topBarMenuSubscription();
    this.menuProfileSubscription();

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.hideMenu();
      });

    this.envName$ = this.store.select(getAppSettings).pipe(
      map(settings => {
        return this.getEnvironmentName(settings?.environment.type);
      })
    );
    this.showEnvironmentMessage$ = this.store.select(getAppSettings).pipe(
      map(settings => {
        return this.showEnvironmentMessage(settings?.environment.type);
      })
    );

    this.sub.add(
      this.store.select(getAppSettings).subscribe(appSettings => {
        if (appSettings) {
          this.cssClassEnv = `env-${appSettings.environment.type.toLowerCase()}`;
        }
      })
    );
  }

  protected menuProfileSubscription() {
    this.sub.add(
      this.layoutService.menuProfileOpen$.subscribe(() => {
        this.hideMenu();
        if (!this.menuProfileOutsideClickListener) {
          this.menuProfileOutsideClickListener = this.renderer.listen(
            'document',
            'click',
            event => {
              const isOutsideClicked = !(
                this.appSidebar.menuProfile.el.nativeElement.isSameNode(
                  event.target
                ) ||
                this.appSidebar.menuProfile.el.nativeElement.contains(
                  event.target
                )
              );
              if (isOutsideClicked) {
                this.hideMenuProfile();
              }
            }
          );
        }
      })
    );
  }

  protected topBarMenuSubscription() {
    this.sub.add(
      this.layoutService.topbarMenuOpen$.subscribe(() => {
        if (!this.topbarMenuOutsideClickListener) {
          this.topbarMenuOutsideClickListener = this.renderer.listen(
            'document',
            'click',
            event => {
              const isOutsideClicked = !(
                this.appTopbar.el.nativeElement.isSameNode(event.target) ||
                this.appTopbar.el.nativeElement.contains(event.target) ||
                this.appTopbar.mobileMenuButton.nativeElement.isSameNode(
                  event.target
                ) ||
                this.appTopbar.mobileMenuButton.nativeElement.contains(
                  event.target
                )
              );
              if (isOutsideClicked) {
                this.hideTopbarMenu();
              }
            }
          );
        }

        if (this.layoutService.state.staticMenuMobileActive) {
          this.blockBodyScroll();
        }
      })
    );
  }

  protected overlayFooterSubscription() {
    this.sub.add(
      this.layoutService.overlayFooterOpen$.subscribe(() => {
        if (!this.footerOutsideClickListener) {
          this.footerOutsideClickListener = this.renderer.listen(
            'document',
            'click',
            event => {
              const isOutsideClicked = !(
                this.appFooter.el.nativeElement.isSameNode(event.target) ||
                this.appFooter.el.nativeElement.contains(event.target)
              );
              if (isOutsideClicked) {
                this.hideFooter();
              }
            }
          );
        }
      })
    );
  }

  protected overlayMenuSubscription() {
    this.sub.add(
      this.layoutService.overlayOpen$.subscribe(() => {
        this.hideTopbarMenu();

        if (!this.menuOutsideClickListener) {
          this.menuOutsideClickListener = this.renderer.listen(
            'document',
            'click',
            event => {
              const isOutsideClicked = !(
                this.appSidebar.el.nativeElement.isSameNode(event.target) ||
                this.appSidebar.el.nativeElement.contains(event.target) ||
                this.appTopbar.menuButton.nativeElement.isSameNode(
                  event.target
                ) ||
                this.appTopbar.menuButton.nativeElement.contains(
                  event.target
                ) ||
                this.appTopbar.menuButtonFullScreen?.nativeElement.isSameNode(
                  event.target
                ) ||
                this.appTopbar.menuButtonFullScreen?.nativeElement.contains(
                  event.target
                )
              );
              if (isOutsideClicked) {
                this.hideMenu();
              }
            }
          );
        }

        if (
          (this.layoutService.isHorizontal() ||
            this.layoutService.isSlim() ||
            this.layoutService.isSlimPlus()) &&
          !this.menuScrollListener
        ) {
          this.menuScrollListener = this.renderer.listen(
            this.appSidebar.menuContainer.nativeElement,
            'scroll',
            () => {
              if (this.layoutService.isDesktop()) {
                this.hideMenu();
              }
            }
          );
        }

        if (this.layoutService.state.staticMenuMobileActive) {
          this.blockBodyScroll();
        }
      })
    );
  }

  ngOnInit(): void {
    this.setNoMargin(this.activatedRoute);
    this.setNoPadding(this.activatedRoute);
    this.sub.add(
      this.translateService
        .stream('bia.language')
        .subscribe(() => this.updateMenuItems())
    );
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.setNoMargin(this.activatedRoute);
        this.setNoPadding(this.activatedRoute);
        this.updateMenuItems();
      });

    this.sub.add(
      this.layoutService.breadcrumbRefresh$.subscribe(() => {
        this.setNoMargin(this.activatedRoute);
        this.setNoPadding(this.activatedRoute);
        this.updateMenuItems();
      })
    );
  }

  protected updateMenuItems() {
    const menuItems = this.createBreadcrumbs(this.activatedRoute.root);
    if (menuItems !== undefined) {
      setTimeout(() => (this.menuItems = menuItems), 0);
    }
  }

  protected createBreadcrumbs(
    route: ActivatedRoute,
    url = '',
    breadcrumbs: MenuItem[] = [{ icon: 'pi pi-home', routerLink: ['/'] }]
  ): MenuItem[] | undefined {
    const children: ActivatedRoute[] = route.children;

    if (children.length === 0) {
      return breadcrumbs;
    }

    for (const child of children) {
      const routeURL: string = child.snapshot.url
        .map(segment => segment.path)
        .join('/');
      if (routeURL !== '') {
        url += `/${routeURL}`;
      }

      const label = child.snapshot.data[ROUTE_DATA_BREADCRUMB];
      if (label) {
        if (child.snapshot.data[ROUTE_DATA_CAN_NAVIGATE] === true) {
          breadcrumbs.push({
            label: this.translateService.instant(label),
            routerLink: [url],
          });
        } else {
          breadcrumbs.push({ label: this.translateService.instant(label) });
        }
      }

      return this.createBreadcrumbs(child, url, breadcrumbs);
    }
  }

  protected setNoMargin(activatedRoute: ActivatedRoute, firstPass = true) {
    if (firstPass) {
      this.noMargin = false;
    }

    const children: ActivatedRoute[] = activatedRoute.children;

    if (activatedRoute.snapshot.data[ROUTE_DATA_NO_MARGIN] === true) {
      this.noMargin = true;
    }

    for (const child of children) {
      this.setNoMargin(child, false);
    }
  }

  protected setNoPadding(activatedRoute: ActivatedRoute, firstPass = true) {
    if (firstPass) {
      this.noPadding = false;
    }

    const children: ActivatedRoute[] = activatedRoute.children;

    if (activatedRoute.snapshot.data[ROUTE_DATA_NO_PADDING] === true) {
      this.noPadding = true;
    }

    for (const child of children) {
      this.setNoPadding(child, false);
    }
  }

  blockBodyScroll(): void {
    if (document.body.classList) {
      document.body.classList.add('blocked-scroll');
    } else {
      document.body.className += ' blocked-scroll';
    }
  }

  unblockBodyScroll(): void {
    if (document.body.classList) {
      document.body.classList.remove('blocked-scroll');
    } else {
      document.body.className = document.body.className.replace(
        new RegExp(
          '(^|\\b)' + 'blocked-scroll'.split(' ').join('|') + '(\\b|$)',
          'gi'
        ),
        ' '
      );
    }
  }

  hideMenu() {
    this.layoutService.state.overlayMenuActive = false;
    this.layoutService.state.staticMenuMobileActive = false;
    this.layoutService.state.menuHoverActive = false;
    this.menuService.reset();

    if (this.menuOutsideClickListener) {
      this.menuOutsideClickListener();
      this.menuOutsideClickListener = null;
    }

    if (this.menuScrollListener) {
      this.menuScrollListener();
      this.menuScrollListener = null;
    }
    this.unblockBodyScroll();
  }

  hideFooter() {
    this.layoutService.state.overlayFooterActive = false;

    if (this.footerOutsideClickListener) {
      this.footerOutsideClickListener();
      this.footerOutsideClickListener = null;
    }
  }

  hideTopbarMenu() {
    this.layoutService.state.topbarMenuActive = false;

    if (this.topbarMenuOutsideClickListener) {
      this.topbarMenuOutsideClickListener();
      this.topbarMenuOutsideClickListener = null;
    }
  }

  hideMenuProfile() {
    this.layoutService.state.menuProfileActive = false;

    if (this.menuProfileOutsideClickListener) {
      this.menuProfileOutsideClickListener();
      this.menuProfileOutsideClickListener = null;
    }
  }

  get containerClass() {
    const styleClass: { [key: string]: any } = {
      /* eslint-disable @typescript-eslint/naming-convention */
      'layout-overlay': this.layoutService.config().menuMode === 'overlay',
      'layout-footer-overlay':
        this.layoutService.config().footerMode === 'overlay',
      'layout-static': this.layoutService.config().menuMode === 'static',
      'layout-slim': this.layoutService.config().menuMode === 'slim',
      'layout-slim-plus': this.layoutService.config().menuMode === 'slim-plus',
      'layout-horizontal':
        this.layoutService.config().menuMode === 'horizontal',
      'layout-reveal': this.layoutService.config().menuMode === 'reveal',
      'layout-drawer': this.layoutService.config().menuMode === 'drawer',
      'layout-static-inactive':
        this.layoutService.state.staticMenuDesktopInactive &&
        this.layoutService.config().menuMode === 'static',
      'layout-overlay-active': this.layoutService.state.overlayMenuActive,
      'layout-footer-overlay-active':
        this.layoutService.state.overlayFooterActive,
      'layout-mobile-active': this.layoutService.state.staticMenuMobileActive,
      'layout-sidebar-active': this.layoutService.state.sidebarActive,
      'layout-sidebar-anchored': this.layoutService.state.anchored,
      'layout-topbar-menu-active': this.layoutService.state.topbarMenuActive,
      'layout-menu-profile-active': this.layoutService.state.menuProfileActive,
      'layout-content-wrapper-fullscreen': this.layoutService.state.fullscreen,
      'layout-content-wrapper-iframe': this.layoutService.state.isInIframe,
    };
    styleClass['layout-menu-' + this.layoutService.config().colorScheme] = true;
    styleClass[
      'layout-menu-profile-' + this.layoutService.config().menuProfilePosition
    ] = true;
    styleClass[this.cssClassEnv] = true;
    /* eslint-enable @typescript-eslint/naming-convention */
    return styleClass;
  }

  ngOnDestroy() {
    if (this.overlayMenuOpenSubscription) {
      this.overlayMenuOpenSubscription.unsubscribe();
    }

    if (this.menuOutsideClickListener) {
      this.menuOutsideClickListener();
    }

    this.sub.unsubscribe();
  }

  onLanguageChange(lang: string) {
    this.biaTranslation.loadAndChangeLanguage(lang);
  }

  onThemeChange(theme: string) {
    this.biaTheme.changeTheme(theme);
  }

  public showEnvironmentMessage(environmentType: EnvironmentType | undefined) {
    return environmentType !== EnvironmentType.PRD;
  }

  protected getEnvironmentName(environmentType: EnvironmentType | undefined) {
    switch (environmentType) {
      case EnvironmentType.DEV:
        return 'Development';
      case EnvironmentType.INT:
        return 'Integration';
      case EnvironmentType.UAT:
        return 'User acceptance test';
      case EnvironmentType.PRA:
        return 'Disaster recovery plan';
      case EnvironmentType.PPD:
        return 'Pre-production';
      case EnvironmentType.PRD:
        return 'Production';
      default:
        return 'Inconnu';
    }
  }
}
