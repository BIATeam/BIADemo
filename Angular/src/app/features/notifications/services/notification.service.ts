import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { Notification } from '../model/notification';
import { getCurrentNotification, getNotificationLoadingGet } from '../store/notification.state';
import { load } from '../store/notifications-actions';

@Injectable({
    providedIn: 'root'
})
export class NotificationService {
    constructor(
        private store: Store<AppState>,
    ) {
        this.InitSub();
        this.loading$ = this.store.select(getNotificationLoadingGet);
        this.notification$ = this.store.select(getCurrentNotification);
    }
    private _currentNotification: Notification;
    private _currentNotificationId: number;
    private sub = new Subscription();
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
        this.store.dispatch(load({ id: id }));
    }

    public get currentNotificationData(): string {
        return this._currentNotification.jData;
    }

    InitSub() {
        this.sub = new Subscription();
        this.sub.add(
            this.store.select(getCurrentNotification).subscribe((notification) => {
                if (notification) {
                    this._currentNotification = notification;
                }
            })
        );
    }
}
