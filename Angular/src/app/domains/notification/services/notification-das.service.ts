import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Notification } from '../model/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector) {
    super(injector, 'Notifications');
  }
}


















