import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Notification } from '../model/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector) {
    super(injector, 'Notifications');
  }

  getUnreadNotificationCount(): Observable<number> {
    return this.http.get<number>(this.route + 'unreadCount');
  }
}


















