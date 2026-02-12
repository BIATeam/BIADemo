import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Observable } from 'rxjs';
import { Notification } from '../model/notification';
import { notificationFieldsConfiguration } from '../public-api';

@Injectable({
  providedIn: 'root',
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector) {
    super(injector, 'Notifications', notificationFieldsConfiguration);
  }
  setUnread(id: number): Observable<Notification> {
    return this.get({
      endpoint: 'setUnread',
      id: id,
    });
  }
}
