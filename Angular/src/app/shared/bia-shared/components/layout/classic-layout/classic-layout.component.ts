import {
  ChangeDetectionStrategy,
  Component,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import {
  APP_SUPPORTED_TRANSLATIONS,
  ROUTE_DATA_BREADCRUMB,
  ROUTE_DATA_CAN_NAVIGATE,
  ROUTE_DATA_NO_MARGIN,
} from 'src/app/shared/constants';
import { BiaNavigation } from '../../../model/bia-navigation';
import { BiaLayoutService } from '../services/layout.service';

@Component({
  selector: 'bia-classic-layout',
  templateUrl: './classic-layout.component.html',
  styleUrls: ['./classic-layout.component.scss'],
  // In order to avoid change detections issues in custom footer / mainBar, stay default here
  changeDetection: ChangeDetectionStrategy.Default,
})
export class ClassicLayoutComponent implements OnInit, OnDestroy {
  @HostBinding('class.no-margin') noMargin = false;
  @Input() version = 'v0.0.0-dev';
  @Input() appTitle = 'BIA';
  @Input() menus: BiaNavigation[];
  @Input() username?: string;
  @Input() headerLogos: string[];
  @Input() footerLogo = 'assets/bia/img/Footer.png';
  @Input() supportedLangs = APP_SUPPORTED_TRANSLATIONS;
  @Input() allowThemeChange = true;
  @Input() companyName = 'BIA';
  @Input() helpUrl?: string;
  @Input() reportUrl?: string;
  @Input() enableNotifications?: boolean;

  menuItems: MenuItem[];
  protected sub = new Subscription();

  constructor(
    protected biaTranslation: BiaTranslationService,
    protected biaTheme: BiaThemeService,
    public layoutService: BiaLayoutService,
    protected translateService: TranslateService,
    protected router: Router,
    protected activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.setNoMargin(this.activatedRoute);
    this.sub.add(
      this.translateService
        .stream('bia.language')
        .subscribe(() => this.updateMenuItems())
    );
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.setNoMargin(this.activatedRoute);
        this.updateMenuItems();
      });

    this.sub.add(
      this.layoutService.breadcrumbRefresh$.subscribe(() => {
        this.setNoMargin(this.activatedRoute);
        this.updateMenuItems();
      })
    );
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

  protected updateMenuItems() {
    const menuItems = this.createBreadcrumbs(this.activatedRoute.root);
    if (menuItems !== undefined) {
      this.menuItems = menuItems;
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
      return;
    }

    for (const child of children) {
      this.setNoMargin(child, false);
    }
  }
}
