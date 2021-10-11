import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { create } from '../../store/notifications-actions';
import { Notification } from '../../model/notification';
import { AppState } from 'src/app/store/state';
import { NotificationOptionsService } from '../../services/notification-options.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-notification-new',
  templateUrl: './notification-new.component.html',
  styleUrls: ['./notification-new.component.scss']
})
export class NotificationNewComponent implements OnInit {

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    public notificationOptionsService: NotificationOptionsService,
  ) {}

  ngOnInit() {
    this.notificationOptionsService.loadAllOptions();
  }

  onSubmitted(notificationToCreate: Notification) {
    this.store.dispatch(create({ notification: notificationToCreate }));
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
