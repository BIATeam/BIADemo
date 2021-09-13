import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { update } from '../../store/notifications-actions';
import { Observable, Subscription } from 'rxjs';
import { Notification } from '../../model/notification';
import { AppState } from 'src/app/store/state';
import { NotificationService } from '../../services/notification.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-notification-detail',
  templateUrl: './notification-detail.component.html',
  styleUrls: ['./notification-detail.component.scss']
})
export class NotificationDetailComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  private sub = new Subscription();
  loading$: Observable<boolean>;
  notification$: Observable<Notification | undefined>;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public notificationService: NotificationService,
  ) { }

  ngOnInit() {
    this.notification$ = this.notificationService.notification$;
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
    this.router.navigate(['./edit'], { relativeTo: this.activatedRoute });
    //this.router.navigate(['./edit'], { relativeTo: this.activatedRoute });
  }
}
