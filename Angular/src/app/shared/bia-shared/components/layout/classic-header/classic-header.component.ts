import { Component, ChangeDetectionStrategy, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { BiaClassicLayoutService } from '../classic-layout/bia-classic-layout.service';
import { Platform } from '@angular/cdk/platform';
import { MenuItem, Message } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { BiaNavigation } from '../../../model/bia-navigation';
import { Subscription, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { THEME_LIGHT, THEME_DARK } from 'src/app/shared/constants';
import { EnvironmentType } from 'src/app/domains/environment-configuration/model/environment-configuration';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { getUnreadNotificationCount } from 'src/app/domains/notification/store/notification.state';
import { loadUnreadNotificationIds } from 'src/app/domains/notification/store/notifications-actions';
import { OptionDto } from '../../../model/option-dto';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { Router } from '@angular/router';
import { UserData } from '../../../model/auth-info';

@Component({
  selector: 'bia-classic-header',
  templateUrl: './classic-header.component.html',
  styleUrls: ['./classic-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ClassicHeaderComponent implements OnDestroy {
  @Input()
  set username(name: string | undefined) {
    if (name) {
      this.usernameParam = { name };
    }
    this.buildTopBarMenu();
  }
  @Input() appTitle: string;
  @Input() version: string;
  @Input()
  set environmentType(env: EnvironmentType) {
    if (env) {
      this.cssClassEnv = `env-${env.toLowerCase()}`;
    }
  }
  @Input()
  set menus(navigations: BiaNavigation[]) {
    if (navigations && navigations.length > 0) {
      this.navigations = navigations;
      this.buildNavigation();
    }
  }

  @Input() logos: string[];
  @Input() supportedLangs: string[];
  @Input() allowThemeChange?: boolean;
  @Input() helpUrl?: string;
  @Input() reportUrl?: string;
  @Input() enableNotifications?: boolean;

  currentSite: OptionDto;
  currentRole: OptionDto;
  _userData: UserData
  get userData(): UserData {
    return this._userData;
  }
  @Input()
  set userData(value: UserData) {
    this._userData = value;
    this.initDropdownSite();
    this.initDropdownRole();
  }

  @Output() language = new EventEmitter<string>();
  @Output() theme = new EventEmitter<string>();
  @Output() siteChange = new EventEmitter<number>();
  @Output() setDefaultSite = new EventEmitter<number>();
  @Output() roleChange = new EventEmitter<number>();
  @Output() setDefaultRole = new EventEmitter<number>();

  usernameParam: { name: string };
  navigations: BiaNavigation[];
  fullscreenMode = false;
  isIE = this.platform.TRIDENT;
  urlAppIcon = environment.urlAppIcon;
  urlDMIndex = environment.urlDMIndex;
  displaySiteList = false;
  displayRoleList = false;
  cssClassEnv: string;
  singleRoleMode = environment.singleRoleMode;

  private sub = new Subscription();

  topBarMenuItems: any; // MenuItem[]; // bug v9 primeNG
  navMenuItems: MenuItem[];
  appIcon$: Observable<string>;

  unreadNotificationCount$: Observable<number>;

  constructor(
    public layoutService: BiaClassicLayoutService,
    public auth: AuthService,
    public translateService: TranslateService,
    private platform: Platform,
    private store: Store<AppState>,
    private biaMessageService: BiaMessageService,
    private router: Router
  ) {
    this.unreadNotificationCount$ = this.store.select(getUnreadNotificationCount);
    this.store.dispatch(loadUnreadNotificationIds());
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onNotificationClick(message: Message) {
    if (message.data?.route) {
      this.router.navigate(message.data.route);
    } else if (message.data?.notificationId) {
      this.router.navigate(['/notifications/',message.data?.notificationId, 'detail']);
    } else {
      this.router.navigate(['/notifications/']);
    }
    this.biaMessageService.clear('bia-signalR');
  }

  toggleFullscreenMode() {
    this.fullscreenMode = !this.fullscreenMode;
    if (this.fullscreenMode === true) {
      this.layoutService.hideFooter();
      this.layoutService.hideBreadcrumb();
    } else {
      this.layoutService.showFooter();
      this.layoutService.showBreadcrumb();
    }
  }

  refresh() {
    localStorage.clear();
    sessionStorage.clear();
    location.reload();
  }

  openHelp() {
    window.open(this.helpUrl, 'blank');
  }

  openReport() {
    window.open(this.reportUrl, 'blank');
  }

  private onChangeTheme(theme: string) {
    this.theme.emit(theme);
  }

  private onChangeLanguage(lang: string) {
    this.language.emit(lang);
  }

  onSiteChange() {
    this.siteChange.emit(this.currentSite.id);
  }

  onSetDefaultSite() {
    this.setDefaultSite.emit(this.currentSite.id);
  }

  private initDropdownSite() {
    this.displaySiteList = false;
    if (this.userData.currentSiteId > 0 && this.userData.sites && this.userData.sites.length > 1) {
      this.currentSite = this.userData.sites.filter((x) => x.id === this.userData.currentSiteId)[0];
      this.displaySiteList = true;
    }
  }

  onRoleChange() {
    this.roleChange.emit(this.currentRole.id);
  }

  onSetDefaultRole() {
    this.setDefaultRole.emit(this.currentRole.id);
  }

  private initDropdownRole() {
    this.displayRoleList = false;
    if (environment.singleRoleMode)
    {
      if (this.userData.currentRoleIds && this.userData.currentRoleIds.length == 1 && this.userData.roles && this.userData.roles.length > 1) {
        this.currentRole = this.userData.roles.filter((x) => x.id === this.userData.currentRoleIds[0])[0];
        this.displayRoleList = true;
      }
    }
  }

  buildNavigation() {
    const translationKeys = new Array<string>();
    this.navigations.forEach((menu) => {
      if (menu.children) {
        menu.children.forEach((child) => {
          translationKeys.push(child.labelKey);
        });
      }
      translationKeys.push(menu.labelKey);
    });

    this.sub.add(
      this.translateService.stream(translationKeys).subscribe((translations) => {
        this.navMenuItems = [];
        this.navigations.forEach((menu) => {
          const childrenMenuItem: MenuItem[] = [];
          if (menu.children) {
            menu.children.forEach((child) => {
              childrenMenuItem.push({
                label: translations[child.labelKey],
                routerLink: child.path
              });
            });
          }
          this.navMenuItems.push({
            label: translations[menu.labelKey],
            routerLink: menu.path,
            items: childrenMenuItem.length > 0 ? childrenMenuItem : undefined
          });
        });
      })
    );
  }

  buildTopBarMenu() {
    const translationKeys = [
      'bia.lang.fr',
      'bia.lang.de',
      'bia.lang.es',
      'bia.lang.gb',
      'bia.lang.mx',
      'bia.lang.us',
      'bia.greetings',
      'bia.languages',
      'bia.theme',
      'bia.themeLight',
      'bia.themeDark'
    ];
    this.sub.add(
      this.translateService.stream(translationKeys).subscribe((translations) => {
        const menuItemLang: MenuItem[] = [];

        if (this.supportedLangs) {
          this.supportedLangs.forEach((lang) => {
            menuItemLang.push({
              label: translations['bia.lang.' + lang.split('-')[1].toLowerCase()],
              command: () => {
                this.onChangeLanguage(lang);
              }
            });
          });
        }

        let displayName = '';
        if (this.usernameParam && this.usernameParam.name) {
          displayName = this.usernameParam.name;
        }

        this.topBarMenuItems = [
          {
            label: translations['bia.greetings'] + ' ' + displayName,
            items: [
              [
                {
                  label: translations['bia.languages'],
                  items: menuItemLang
                },
                {
                  label: translations['bia.theme'],
                  items: [
                    {
                      label: translations['bia.themeLight'],
                      command: () => {
                        this.onChangeTheme(THEME_LIGHT);
                      }
                    },
                    {
                      label: translations['bia.themeDark'],
                      command: () => {
                        this.onChangeTheme(THEME_DARK);
                      }
                    }
                  ]
                }
              ]
            ]
          }
        ];
      })
    );
  }
}
