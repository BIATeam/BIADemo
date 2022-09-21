import { Injectable, Injector } from '@angular/core';
import { Notification } from '../model/notification';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector) {
    super(injector, 'Notifications');
  }
  setUnread(id: number): Observable<Notification> {
    return this.get({ endpoint: 'setUnread', id: id});
  }
}
