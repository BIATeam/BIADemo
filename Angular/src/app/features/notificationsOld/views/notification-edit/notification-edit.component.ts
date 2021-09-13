import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { AppState } from 'src/app/store/state';
import { Store } from '@ngrx/store';
import { getCurrentNotification } from '../../store/notification.state';
import { Observable } from 'rxjs';
import { Notification } from '../../model/notification';

@Component({
  selector: 'app-notification-edit',
  templateUrl: './notification-edit.component.html',
  styleUrls: ['./notification-edit.component.scss']
})
export class NotificationEditComponent implements OnInit {
  @Output() displayChange = new EventEmitter<boolean>();
  notification$: Observable<Notification | undefined>;

  constructor(private location: Location, private store: Store<AppState>) { }

  ngOnInit() {
    this.notification$ = this.store.select(getCurrentNotification);
  }

  onCancelled() {
    this.location.back();
  }
}
