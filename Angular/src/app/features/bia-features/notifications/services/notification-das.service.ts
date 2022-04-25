import { Injectable, Injector } from '@angular/core';
import { Notification } from '../model/notification';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector) {
    super(injector, 'Notifications');
  }
}
