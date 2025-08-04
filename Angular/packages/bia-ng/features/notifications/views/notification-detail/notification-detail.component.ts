import { AsyncPipe, DatePipe, NgIf } from '@angular/common';
import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService, Permission } from 'packages/bia-ng/core/public-api';
import { AuthInfo } from 'packages/bia-ng/models/public-api';
import {
  NotificationTeamWarningComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { ButtonDirective } from 'primeng/button';
import { Observable, Subscription } from 'rxjs';
import { Notification, NotificationData } from '../../model/notification';
import { NotificationService } from '../../services/notification.service';
import { FeatureNotificationsActions } from '../../store/notifications-actions';

@Component({
  selector: 'bia-notification-detail',
  templateUrl: './notification-detail.component.html',
  styleUrls: ['./notification-detail.component.scss'],
  imports: [
    NgIf,
    ButtonDirective,
    NotificationTeamWarningComponent,
    AsyncPipe,
    DatePipe,
    TranslateModule,
    SpinnerComponent,
  ],
})
export class NotificationDetailComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  protected sub = new Subscription();
  canEdit: boolean;
  loading$: Observable<boolean>;
  notification$: Observable<Notification | undefined>;

  constructor(
    protected store: Store<BiaAppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    protected authService: AuthService,
    public notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.notification$ = this.notificationService.crudItem$;
    this.sub.add(
      this.authService.authInfo$.subscribe((authInfo: AuthInfo) => {
        if (authInfo && authInfo.token !== '') {
          this.canEdit = this.authService.hasPermission(
            Permission.Notification_Update
          );
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(notificationToUpdate: Notification) {
    this.store.dispatch(
      FeatureNotificationsActions.update({ notification: notificationToUpdate })
    );
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onEdit() {
    this.router.navigate(['../edit'], { relativeTo: this.activatedRoute });
  }

  canAction(notification: Notification) {
    if (notification.data) {
      if (notification.data.route) {
        return true;
      }
    }
    return false;
  }

  onAction(notification: Notification) {
    if (notification.data) {
      const data: NotificationData = notification.data;
      if (data.route) {
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
      }
    }
  }

  onSetUnread(id: number) {
    this.store.dispatch(FeatureNotificationsActions.setUnread({ id }));
  }
}
