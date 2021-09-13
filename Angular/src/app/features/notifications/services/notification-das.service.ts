import { Injectable, Injector } from '@angular/core';
import { Notification } from '../model/notification';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { LazyLoadEvent } from 'primeng';
import { Observable } from 'rxjs';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { HttpOptions } from 'src/app/core/bia-core/services/generic-das.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationDas extends AbstractDas<Notification> {
  constructor(injector: Injector, private translate: TranslateService) {
    super(injector, 'Notifications');
  }
  getListByPost(event: LazyLoadEvent, endpoint: string = 'all'): Observable<DataResult<Notification[]>> {
    return this.getListItemsByPost<Notification>(event, endpoint).pipe(map(dataResult => {
      dataResult.data.map(notif => {
        notif.type.display = this.translate.instant(`notification.type.${notif.type.display.toLowerCase()}`);
        notif.notifiedRoles.map(role => {
          role.display =  this.translate.instant(`role.${role.display.toLowerCase()}`);
          return role;
        });
        return notif;
      });
      return dataResult;
    }));
  }

  get(id: string | number, options?: HttpOptions): Observable<Notification> {
    return this.getItem<Notification>(id, options).pipe(map( notif => {
      notif.type.display = this.translate.instant(`notification.type.${notif.type.display.toLowerCase()}`);
      notif.notifiedRoles.map(role => {
        role.display =  this.translate.instant(`role.${role.display.toLowerCase()}`);
        return role;
      });
      return notif;
    } ));
  }
}
