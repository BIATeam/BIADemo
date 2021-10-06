import { Component, HostBinding, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { callWorkerWithNotification } from '../../store/hangfire-actions';

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
