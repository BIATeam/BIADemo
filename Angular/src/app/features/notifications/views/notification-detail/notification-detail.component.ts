import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update } from '../../store/notifications-actions';
import { Observable, Subscription } from 'rxjs';
import { Notification } from '../../model/notification';
import { AppState } from 'src/app/store/state';
import { NotificationService } from '../../services/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Permission } from 'src/app/shared/permission';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';

@Component({
  selector: 'app-notification-detail',
  templateUrl: './notification-detail.component.html',
  styleUrls: ['./notification-detail.component.scss']
})
export class NotificationDetailComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  private sub = new Subscription();
  canEdit: boolean;
  loading$: Observable<boolean>;
  notification$: Observable<Notification | undefined>;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    public notificationService: NotificationService,
  ) { }

  ngOnInit() {
    this.notification$ = this.notificationService.notification$;
    this.canEdit = this.authService.hasPermission(Permission.Notification_Update);
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(notificationToUpdate: Notification) {
    this.store.dispatch(update({ notification: notificationToUpdate }));
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onEdit() {
    this.router.navigate(['../edit'], { relativeTo: this.activatedRoute });
  }

  canAction (notification: Notification) {
    if (notification.jData) {
      const data = JSON.parse(notification.jData);
      if (data?.route) {
        return true;
      }
    }
    return false;
  }

  onAction(notification: Notification) {
    if (notification.jData) {
      const data = JSON.parse(notification.jData);
      if (data?.route) {
        this.router.navigate(data.route);
      }
    }
  }
}
