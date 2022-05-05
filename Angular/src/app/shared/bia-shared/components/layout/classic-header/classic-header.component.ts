import { Component, ChangeDetectionStrategy, Input, Output, EventEmitter, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { BiaClassicLayoutService } from '../classic-layout/bia-classic-layout.service';
import { Platform } from '@angular/cdk/platform';
import { MenuItem, Message } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { BiaNavigation } from '../../../model/bia-navigation';
import { Subscription, Observable } from 'rxjs';
import { THEME_LIGHT, THEME_DARK } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { getUnreadNotificationCount } from 'src/app/domains/bia-domains/notification/store/notification.state';
import { DomainNotificationsActions } from 'src/app/domains/bia-domains/notification/store/notifications-actions';
import { Router } from '@angular/router';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { allEnvironments } from 'src/environments/all-environments';
import { Toast } from 'primeng/toast';
import { Notification, NotificationData, NotificationType } from 'src/app/domains/bia-domains/notification/model/notification';

@Component({
  selector: 'bia-classic-header',
  templateUrl: './classic-header.component.html',
  styleUrls: ['./classic-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ClassicHeaderComponent implements OnInit, OnDestroy {
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

  @Output() language = new EventEmitter<string>();
  @Output() theme = new EventEmitter<string>();

  usernameParam: { name: string };
  navigations: BiaNavigation[];
  fullscreenMode = false;
  isIE = this.platform.TRIDENT;
  urlAppIcon = allEnvironments.urlAppIcon;
  cssClassEnv: string;
  private sub = new Subscription();

  topBarMenuItems: any; // MenuItem[]; // bug v9 primeNG
  navMenuItems: MenuItem[];
  appIcon$: Observable<string>;

  unreadNotificationCount$: Observable<number>;

  teamTypeSelectors: number[];

  @ViewChild('toast', { static: true }) toast: Toast;
  NotificationType = NotificationType;

  constructor(
    public layoutService: BiaClassicLayoutService,
    public authService: AuthService,
    public translateService: TranslateService,
    private platform: Platform,
    private store: Store<AppState>,
    public biaTranslationService: BiaTranslationService,
    private router: Router
  ) {
  }

  ngOnInit() {
    this.teamTypeSelectors = allEnvironments.teams.filter(t => t.inHeader === true).map(t => t.teamTypeId);

    if (allEnvironments.enableNotifications === true) {
      this.unreadNotificationCount$ = this.store.select(getUnreadNotificationCount);
      this.store.dispatch(DomainNotificationsActions.loadUnreadNotificationIds());
    }
    this.sub.add(
      this.biaTranslationService.appSettings$.subscribe(appSettings => {
        if (appSettings) {
          this.cssClassEnv = `env-${appSettings.environment.type.toLowerCase()}`;
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onNotificationClick(message: Message) {
    if (message.data?.notification) {
      let notification : Notification = message.data.notification;
      let data : NotificationData = notification.data;
      if (data?.route) {
        if (data?.teams) {
          // Auto-switch to teams related to this notification
          data.teams.forEach((team) => {
            this.authService.changeCurrentTeamId(team.teamTypeId, team.team.id);
            if (team.roles)
            {
              this.authService.changeCurrentRoleIds(team.teamTypeId, team.team.id, team.roles.map(r => r.id));
            }
          })
        }
        this.router.navigate(data.route);
      } else if (notification.id) {
        this.router.navigate(['/notifications/', notification.id, 'detail']);
      } else {
        this.router.navigate(['/notifications/']);
      }
      this.removeMessage(message, true);
    }
  }

  onIgnoreClick(message: Message) {
    this.removeMessage(message, true);
  }

  private removeMessage(message: Message, setRead = false) {
    this.toast.messages.splice(this.toast.messages.indexOf(message), 1);

    if (setRead && message.data?.notification?.id > 0) {
      this.store.dispatch(DomainNotificationsActions.setAsRead({ id: message.data.notification.id }))
    }
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
