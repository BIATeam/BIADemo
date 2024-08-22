import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { Notification } from '../model/notification';
import {
  getCurrentNotification,
  getNotificationLoadingGet,
} from '../store/notification.state';
import { FeatureNotificationsActions } from '../store/notifications-actions';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(protected store: Store<AppState>) {
    this.initSub();
    this.loading$ = this.store.select(getNotificationLoadingGet);
    this.notification$ = this.store.select(getCurrentNotification);
  }
  protected _currentNotification: Notification;
  protected _currentNotificationId: number;
  protected sub = new Subscription();
  public loading$: Observable<boolean>;
  public notification$: Observable<Notification>;

  public get currentNotification() {
    if (this._currentNotification?.id === this._currentNotificationId) {
      return this._currentNotification;
    } else {
      return null;
    }
  }

  public get currentNotificationId(): number {
    return this._currentNotificationId;
  }
  public set currentNotificationId(id: number) {
    this._currentNotificationId = Number(id);
    this.store.dispatch(FeatureNotificationsActions.load({ id: id }));
  }

  public get currentNotificationData(): string {
    return this._currentNotification.jData;
  }

  initSub() {
    this.sub = new Subscription();
    this.sub.add(
      this.store.select(getCurrentNotification).subscribe(notification => {
        if (notification) {
          this._currentNotification = notification;
        }
      })
    );
  }
}
