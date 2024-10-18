import { Platform } from '@angular/cdk/platform';
import { DOCUMENT } from '@angular/common';
import {
  Component,
  ElementRef,
  Inject,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { Message } from 'primeng/api';
import { Toast } from 'primeng/toast';
import { Observable, Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import {
  Notification,
  NotificationData,
  NotificationType,
} from 'src/app/domains/bia-domains/notification/model/notification';
import { getUnreadNotificationCount } from 'src/app/domains/bia-domains/notification/store/notification.state';
import { DomainNotificationsActions } from 'src/app/domains/bia-domains/notification/store/notifications-actions';
import { BiaNavigation } from 'src/app/shared/bia-shared/model/bia-navigation';
import { AppState } from 'src/app/store/state';
import { allEnvironments } from 'src/environments/all-environments';
import { BiaLayoutService } from '../../services/layout.service';

@Component({
  selector: 'bia-ultima-top-bar',
  templateUrl: './ultima-top-bar.component.html',
  styleUrls: ['./ultima-top-bar.component.scss'],
})
export class BiaUltimaTopBarComponent implements OnInit, OnDestroy {
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
    @Inject(DOCUMENT) private document: Document,
    public el: ElementRef
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
    if (this.document.exitFullscreen) {
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

  get showToggleStyle(): boolean {
    return this.layoutService.configDisplay().showToggleStyle;
  }

  toggleStyle() {
    this.layoutService.toggleStyle();
  }
}
