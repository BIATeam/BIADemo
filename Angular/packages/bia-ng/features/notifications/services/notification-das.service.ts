import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Observable } from 'rxjs';
import { Notification } from '../model/notification';

@Injectable({
  providedIn: 'root',
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector) {
    super(injector, 'Notifications');
  }
  setUnread(id: number): Observable<Notification> {
    return this.get({
      endpoint: 'setUnread',
      id: id,
    });
  }
}
