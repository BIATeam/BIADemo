import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { Notification } from '../../model/notification';
import { NotificationType } from '../../model/notification-type';
import { getNotificationLoadingGet } from '../../store/notification.state';

@Component({
  selector: 'app-notification-form',
  templateUrl: './notification-form.component.html',
  styleUrls: ['./notification-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotificationFormComponent implements OnInit {
  @Input() notification: Notification;
  @Output() cancel = new EventEmitter();
  NotificationType = <NotificationType>{};

  loading$: Observable<boolean>;

  constructor(private store: Store<AppState>) {
  }

  ngOnInit() {
    this.loading$ = this.store.select(getNotificationLoadingGet);
  }

  onCancel() {
    this.cancel.next();
  }
}
