import { ChangeDetectionStrategy, Component, Input, OnInit, OnDestroy, EventEmitter, Output, HostBinding } from '@angular/core';
import { BiaClassicLayoutService } from './bia-classic-layout.service';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { MenuItem } from 'primeng/api';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { BiaNavigation } from '../../../model/bia-navigation';
import { ROUTE_DATA_CAN_NAVIGATE, ROUTE_DATA_BREADCRUMB, APP_SUPPORTED_TRANSLATIONS, ROUTE_DATA_NO_MARGIN } from 'src/app/shared/constants';
import { Subscription } from 'rxjs';
import { Site } from 'src/app/domains/site/model/site';
import { EnvironmentType } from 'src/app/domains/environment-configuration/model/environment-configuration';
import { Role } from 'src/app/domains/role/model/role';

@Component({
  selector: 'bia-classic-layout',
  templateUrl: './classic-layout.component.html',
  styleUrls: ['./classic-layout.component.scss'],
  providers: [BiaClassicLayoutService],
  // In order to avoid change detections issues in custom footer / mainBar, stay default here
  changeDetection: ChangeDetectionStrategy.Default
})
export class ClassicLayoutComponent implements OnInit, OnDestroy {
  @HostBinding('class.no-margin') noMargin = false;
  @Input() version = 'v0.0.0-dev';
  @Input() appTitle = 'BIA';
  @Input() menus: BiaNavigation[];
  @Input() username?: string;
  @Input() headerLogos: string[];
  @Input() footerLogo = 'assets/bia/Footer.png';
  @Input() supportedLangs = APP_SUPPORTED_TRANSLATIONS;
  @Input() allowThemeChange = true;
  @Input() companyName = 'BIA';
  @Input() helpUrl?: string;
  @Input() reportUrl?: string;
  @Input() sites: Site;
  @Input() siteId: number;
  @Input() roles: Role[];
  @Input() roleId: number;
  @Input() environmentType: EnvironmentType;

  @Output() siteChange = new EventEmitter<number>();
  @Output() roleChange = new EventEmitter<number>();
  @Output() setDefaultSite = new EventEmitter<number>();
  @Output() setDefaultRole = new EventEmitter<number>();

  menuItems: MenuItem[];
  private sub = new Subscription();

  constructor(
    private biaTranslation: BiaTranslationService,
    private biaTheme: BiaThemeService,
    public layoutService: BiaClassicLayoutService,
    private translateService: TranslateService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.setNoMargin(this.activatedRoute);
    this.sub.add(this.translateService.stream('bia.languages').subscribe(() => this.updateMenuItems()));
    this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event) => {
      this.setNoMargin(this.activatedRoute);
      this.updateMenuItems();
    });

    this.layoutService.breadcrumbRefresh$.subscribe((val) => {
      this.setNoMargin(this.activatedRoute);
      this.updateMenuItems();
    });
  }

  ngOnDestroy(): void {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onLanguageChange(lang: string) {
    this.biaTranslation.loadAndChangeLanguage(lang);
  }

  onThemeChange(theme: string) {
    this.biaTheme.changeTheme(theme);
  }

  onSiteChange(siteId: number) {
    this.siteChange.emit(siteId);
  }

  onRoleChange(roleId: number) {
    this.roleChange.emit(roleId);
  }

  onSetDefaultSite(siteId: number) {
    this.setDefaultSite.emit(siteId);
  }

  onSetDefaultRole(roleId: number) {
    this.setDefaultRole.emit(roleId);
  }

  private updateMenuItems() {
    const menuItems = this.createBreadcrumbs(this.activatedRoute.root);
    if (menuItems !== undefined) {
      this.menuItems = menuItems;
    }
  }

  private createBreadcrumbs(
    route: ActivatedRoute,
    url = '',
    breadcrumbs: MenuItem[] = [{ icon: 'pi pi-home', routerLink: ['/'] }]
  ): MenuItem[] | undefined {
    const children: ActivatedRoute[] = route.children;

    if (children.length === 0) {
      return breadcrumbs;
    }

    for (const child of children) {
      const routeURL: string = child.snapshot.url.map((segment) => segment.path).join('/');
      if (routeURL !== '') {
        url += `/${routeURL}`;
      }

      const label = child.snapshot.data[ROUTE_DATA_BREADCRUMB];
      if (label) {
        if (child.snapshot.data[ROUTE_DATA_CAN_NAVIGATE] === true) {
          breadcrumbs.push({ label: this.translateService.instant(label), routerLink: [url] });
        } else {
          breadcrumbs.push({ label: this.translateService.instant(label) });
        }
      }

      return this.createBreadcrumbs(child, url, breadcrumbs);
    }
  }

  private setNoMargin(activatedRoute: ActivatedRoute, firstPass = true) {

    if (firstPass) {
      this.noMargin = false;
    }

    const children: ActivatedRoute[] = activatedRoute.children;

    if (activatedRoute.snapshot.data[ROUTE_DATA_NO_MARGIN] === true) {
      this.noMargin = true;
      return;
    }

    for (const child of children) {
      this.setNoMargin(child, false);
    }
  }
}
