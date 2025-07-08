import { Platform } from '@angular/cdk/platform';
import {
  AsyncPipe,
  DOCUMENT,
  NgClass,
  NgFor,
  NgIf,
  NgSwitch,
  NgSwitchCase,
  NgSwitchDefault,
} from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  Inject,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { PrimeTemplate, ToastMessageOptions } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { Ripple } from 'primeng/ripple';
import { Toast } from 'primeng/toast';
import { Tooltip } from 'primeng/tooltip';
import { Observable, Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import {
  Notification,
  NotificationData,
  NotificationType,
} from 'src/app/domains/bia-domains/notification/model/notification';
import { NotificationModule } from 'src/app/domains/bia-domains/notification/notification.module';
import { getUnreadNotificationCount } from 'src/app/domains/bia-domains/notification/store/notification.state';
import { DomainNotificationsActions } from 'src/app/domains/bia-domains/notification/store/notifications-actions';
import { BiaNavigation } from 'src/app/shared/bia-shared/model/bia-navigation';
import { AppState } from 'src/app/store/state';
import { allEnvironments } from 'src/environments/all-environments';
import { BiaOnlineOfflineIconComponent } from '../../../bia-online-offline-icon/bia-online-offline-icon.component';
import { BiaTeamSelectorComponent } from '../../../bia-team-selector/bia-team-selector.component';
import { NotificationTeamWarningComponent } from '../../../notification-team-warning/notification-team-warning.component';
import { IeWarningComponent } from '../../ie-warning/ie-warning.component';
import { BiaLayoutService } from '../../services/layout.service';

@Component({
  selector: 'bia-ultima-topbar',
  templateUrl: './ultima-topbar.component.html',
  styleUrls: ['./ultima-topbar.component.scss'],
  imports: [
    RouterLink,
    Ripple,
    NgIf,
    IeWarningComponent,
    NgFor,
    BiaTeamSelectorComponent,
    Tooltip,
    BiaOnlineOfflineIconComponent,
    Toast,
    PrimeTemplate,
    NgSwitch,
    NgSwitchCase,
    NgSwitchDefault,
    ButtonDirective,
    NotificationTeamWarningComponent,
    NgClass,
    AsyncPipe,
    TranslateModule,
    NotificationModule,
  ],
})
export class BiaUltimaTopbarComponent
  implements OnInit, OnDestroy, AfterViewInit
{
  @Input() appTitle: string;
  @Input() version: string;
  @Input() helpUrl?: string;
  @Input() reportUrl?: string;
  @Input() enableNotifications?: boolean;

  usernameParam: { name: string };
  navigations: BiaNavigation[];
  fullscreenMode = false;
  isIE = this.platform.TRIDENT;
  urlAppIcon = allEnvironments.urlAppIcon;
  protected sub = new Subscription();

  appIcon$: Observable<string>;

  unreadNotificationCount$: Observable<number>;

  teamTypeSelectors: any[];

  @ViewChild('menuButton') menuButton: ElementRef;
  @ViewChild('menuButtonFullscreen', { static: false })
  menuButtonFullScreen: ElementRef;

  @ViewChild('mobileMenuButton') mobileMenuButton!: ElementRef;

  @ViewChild('toast', { static: true }) toast: Toast;
  notificationType = NotificationType;

  constructor(
    public layoutService: BiaLayoutService,
    public authService: AuthService,
    public translateService: TranslateService,
    protected platform: Platform,
    protected store: Store<AppState>,
    public biaTranslationService: BiaTranslationService,
    protected router: Router,
    @Inject(DOCUMENT) protected document: Document,
    public el: ElementRef,
    protected readonly biaMessageService: BiaMessageService,
    protected renderer: Renderer2
  ) {}

  ngOnInit() {
    this.teamTypeSelectors = this.authService
      .getLoginParameters()
      .teamsConfig.filter(t => t.inHeader === true);

    if (allEnvironments.enableNotifications === true) {
      this.unreadNotificationCount$ = this.store.select(
        getUnreadNotificationCount
      );
      this.store.dispatch(
        DomainNotificationsActions.loadUnreadNotificationIds()
      );
    }
  }

  ngAfterViewInit(): void {
    this.positionClearButton();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onNotificationClick(message: ToastMessageOptions) {
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

  onIgnoreClick(message: ToastMessageOptions) {
    this.removeMessage(message, true);
  }

  protected removeMessage(message: ToastMessageOptions, setRead = false) {
    this.toast.messages?.splice(this.toast.messages?.indexOf(message), 1);

    if (setRead && message.data?.notification?.id > 0) {
      this.store.dispatch(
        DomainNotificationsActions.setAsRead({
          id: message.data.notification.id,
        })
      );
    }
  }

  clearAllMessages() {
    this.biaMessageService.clear('bia');
  }

  toggleFullscreenMode() {
    this.fullscreenMode = !this.fullscreenMode;
    if (this.fullscreenMode === true) {
      this.layoutService.hideFooter();
      this.layoutService.menuClose();
      this.openFullscreen();
    } else {
      this.layoutService.showFooter();
      this.layoutService.menuOpen();
      this.closeFullscreen();
    }
    this.layoutService.setFullscreen(this.fullscreenMode);
  }

  openFullscreen() {
    const elem = document.documentElement;
    if (elem.requestFullscreen) {
      elem.requestFullscreen();
    }
  }

  closeFullscreen() {
    if (this.document.exitFullscreen && this.document.fullscreenElement) {
      this.document.exitFullscreen();
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

  onMenuButtonClick() {
    this.layoutService.onMenuToggle();
  }

  onMobileTopbarMenuButtonClick() {
    this.layoutService.onTopbarMenuToggle();
  }

  get mobileTopbarActive(): boolean {
    return this.layoutService.state.topbarMenuActive;
  }

  openSettings() {
    this.layoutService.openConfigSidebar();
  }

  positionClearButton() {
    const parentElement = document
      .getElementById('toast')
      ?.querySelector('.p-toast.p-component');

    if (parentElement) {
      const clearButton = document.getElementById('clearButton');
      this.renderer.insertBefore(
        parentElement,
        clearButton,
        parentElement.firstChild
      );
    }
  }
}
