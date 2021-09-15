import { Component, HostBinding, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { callWorkerWithNotification } from 'src/app/features/notificationsOld/store/notifications-actions';
import { AppState } from 'src/app/store/state';

@Component({
  selector: 'app-hangfire-index',
  templateUrl: './hangfire-index.component.html',
  styleUrls: ['./hangfire-index.component.scss']
})
export class HangfireIndexComponent implements OnInit {
  @HostBinding('class.bia-flex') flex = true;

  constructor(private store: Store<AppState>,) {
  }

  ngOnInit(): void {
  }

  callWorkerWithNotification() {
    this.store.dispatch(callWorkerWithNotification());
  }
}
