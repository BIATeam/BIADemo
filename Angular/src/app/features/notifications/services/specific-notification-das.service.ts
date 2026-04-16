import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Observable } from 'rxjs';
import { SpecificNotification } from '../model/specific-notification';
import { specificNotificationFieldsConfiguration } from '../model/specific-notification-list-item';

@Injectable()
export class SpecificNotificationDas extends AbstractDas<SpecificNotification> {
  constructor(injector: Injector) {
    super(injector, 'Notifications', specificNotificationFieldsConfiguration);
  }

  setUnread(id: number): Observable<SpecificNotification> {
    return this.get({ endpoint: 'setUnread', id });
  }
}
