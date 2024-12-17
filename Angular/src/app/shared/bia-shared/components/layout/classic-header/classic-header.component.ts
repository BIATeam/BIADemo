import { Platform } from '@angular/cdk/platform';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { MegaMenuItem, MenuItem, Message } from 'primeng/api';
import { Toast } from 'primeng/toast';
import { Observable, Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import {
  Notification,
  NotificationData,
  NotificationType,
} from 'src/app/domains/bia-domains/notification/model/notification';
import { getUnreadNotificationCount } from 'src/app/domains/bia-domains/notification/store/notification.state';
import { DomainNotificationsActions } from 'src/app/domains/bia-domains/notification/store/notifications-actions';
import { THEME_DARK, THEME_LIGHT } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { allEnvironments } from 'src/environments/all-environments';
import { BiaNavigation } from '../../../model/bia-navigation';
import { BiaLayoutService } from '../services/layout.service';

@Component({
  selector: 'bia-classic-header',
  templateUrl: './classic-header.component.html',
  styleUrls: ['./classic-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
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
  protected sub = new Subscription();

  topBarMenuItems: MegaMenuItem[];
  navMenuItems: MenuItem[];
  appIcon$: Observable<string>;

  unreadNotificationCount$: Observable<number>;

  teamTypeSelectors: any[];

  @ViewChild('toast', { static: true }) toast: Toast;
  notificationType = NotificationType;

  constructor(
    protected readonly biaTheme: BiaThemeService,
    public layoutService: BiaLayoutService,
    public authService: AuthService,
    public translateService: TranslateService,
    protected platform: Platform,
    protected store: Store<AppState>,
    public biaTranslationService: BiaTranslationService,
    protected router: Router
  ) {}

  ngOnInit() {
    this.teamTypeSelectors = allEnvironments.teams.filter(
      t => t.inHeader === true
    );

    if (allEnvironments.enableNotifications === true) {
      this.unreadNotificationCount$ = this.store.select(
        getUnreadNotificationCount
      );
      this.store.dispatch(
        DomainNotificationsActions.loadUnreadNotificationIds()
      );
    }
    this.sub.add(
      this.biaTranslationService.appSettings$.subscribe(appSettings => {
        if (appSettings) {
          this.cssClassEnv = `env-${appSettings.environment.type.toLowerCase()}`;
        }
      })
    );
    this.buildTopBarMenu();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onNotificationClick(message: Message) {
    if (message.data?.notification) {
      const notification: Notification = message.data.notification;
      const data: NotificationData | undefined = notification.data;
      if (data?.route) {
        if (data?.teams) {
          // Auto-switch to teams related to this notification
          data.teams.forEach(team => {
            this.authService.changeCurrentTeamId(team.teamTypeId, team.team.id);
            if (team.roles) {
              this.authService.changeCurrentRoleIds(
                team.teamTypeId,
                team.team.id,
                team.roles.map(r => r.id)
              );
            }
          });
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

  protected removeMessage(message: Message, setRead = false) {
    this.toast.messages?.splice(this.toast.messages?.indexOf(message), 1);

    if (setRead && message.data?.notification?.id > 0) {
      this.store.dispatch(
        DomainNotificationsActions.setAsRead({
          id: message.data.notification.id,
        })
      );
    }
  }

  toggleFullscreenMode() {
    this.fullscreenMode = !this.fullscreenMode;
    if (this.fullscreenMode === true) {
      this.layoutService.hideFooter();
      this.layoutService.hideBreadcrumb();
      this.layoutService.state.fullscreen = true;
    } else {
      this.layoutService.showFooter();
      this.layoutService.showBreadcrumb();
      this.layoutService.state.fullscreen = false;
    }
  }

  refresh() {
    this.layoutService.clearSession();
    location.reload();
  }

  openHelp() {
    window.open(this.helpUrl, 'blank');
  }

  openReport() {
    window.open(this.reportUrl, 'blank');
  }

  protected onChangeTheme(theme: string) {
    this.biaTheme.changeTheme(theme);
    this.theme.emit(theme);
  }

  protected onChangeLanguage(lang: string) {
    this.language.emit(lang);
  }

  buildNavigation() {
    const translationKeys = new Array<string>();
    this.navigations.forEach(menu => {
      if (menu.children) {
        menu.children.forEach(child => {
          translationKeys.push(child.labelKey);
        });
      }
      translationKeys.push(menu.labelKey);
    });

    this.navMenuItems = this.layoutService.mapNavigationToMenuItems(
      this.navigations
    );

    this.sub.add(
      this.translateService.stream(translationKeys).subscribe(translations => {
        this.layoutService.processMenuTranslation(
          this.navMenuItems,
          translations
        );
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
      'bia.language',
      'bia.theme',
      'bia.themeLight',
      'bia.themeDark',
    ];
    this.sub.add(
      this.translateService.stream(translationKeys).subscribe(translations => {
        const menuItemLang: MenuItem[] = [];
        if (this.supportedLangs) {
          this.supportedLangs.forEach(lang => {
            menuItemLang.push({
              label:
                translations['bia.lang.' + lang.split('-')[1].toLowerCase()],
              command: () => {
                this.onChangeLanguage(lang);
              },
            });
          });

          menuItemLang.sort((a, b) => {
            const labelA = a.label || '';
            const labelB = b.label || '';
            return labelA.localeCompare(labelB);
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
                  label: translations['bia.language'],
                  items: menuItemLang,
                },
                {
                  label: translations['bia.theme'],
                  items: [
                    {
                      label: translations['bia.themeLight'],
                      command: () => {
                        this.onChangeTheme(THEME_LIGHT);
                      },
                    },
                    {
                      label: translations['bia.themeDark'],
                      command: () => {
                        this.onChangeTheme(THEME_DARK);
                      },
                    },
                  ],
                },
              ],
            ],
          },
        ];
      })
    );
  }

  get showToggleStyle(): boolean {
    return this.layoutService.configDisplay().showToggleStyle;
  }

  toggleStyle() {
    this.layoutService.toggleStyle();
  }
}
