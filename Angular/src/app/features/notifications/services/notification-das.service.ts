import { Injectable, Injector } from '@angular/core';
import { Notification } from '../model/notification';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector, private translate: TranslateService) {
    super(injector, 'Notifications');
  }
  translateItem(item: Notification) {
    item.type.display = this.translate.instant(`notification.type.${item.type.display.toLowerCase()}`);
    item.notifiedRoles.map(role => {
      role.display = this.translate.instant(`role.${role.display.toLowerCase()}`);
      return role;
    });
    return item;
  }
}
