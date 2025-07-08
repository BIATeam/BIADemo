import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { Notification } from '../model/notification';
import { NotificationListItem } from '../model/notification-list-item';
import { notificationCRUDConfiguration } from '../notification.constants';
import { FeatureNotificationsStore } from '../store/notification.state';
import { FeatureNotificationsActions } from '../store/notifications-actions';
import { NotificationDas } from './notification-das.service';
import { NotificationOptionsService } from './notification-options.service';
import { NotificationsSignalRService } from './notification-signalr.service';

@Injectable({
  providedIn: 'root',
})
export class NotificationService extends CrudItemService<
  NotificationListItem,
  Notification
> {
  _updateSuccessActionType = FeatureNotificationsActions.loadAllByPost.type;
  _createSuccessActionType = FeatureNotificationsActions.loadAllByPost.type;
  _updateFailureActionType = FeatureNotificationsActions.failure.type;

  constructor(
    protected store: Store<AppState>,
    public dasService: NotificationDas,
    public signalRService: NotificationsSignalRService,
    public optionsService: NotificationOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Notification : adapt the parent Key tothe context. It can be null if root crud
    return [this.authService.getCurrentTeamId(TeamTypeId.Site)];
  }

  public getFeatureName() {
    return notificationCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<NotificationListItem[]> = this.store.select(
    FeatureNotificationsStore.getAllNotifications
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureNotificationsStore.getNotificationsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureNotificationsStore.getNotificationLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureNotificationsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Notification> = this.store.select(
    FeatureNotificationsStore.getCurrentNotification
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(notification => notification?.title?.toString() ?? '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureNotificationsStore.getNotificationLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureNotificationsActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureNotificationsActions.loadAllByPost({ event }));
  }
  public create(crudItem: Notification) {
    // TODO after creation of CRUD Notification : map parent Key on the corresponding field
    let indexParent = 0;
    crudItem.siteId = this.getParentIds()[indexParent++];
    this.store.dispatch(
      FeatureNotificationsActions.create({ notification: crudItem })
    );
  }
  public update(crudItem: Notification) {
    this.store.dispatch(
      FeatureNotificationsActions.update({ notification: crudItem })
    );
  }
  public remove(id: any) {
    this.store.dispatch(FeatureNotificationsActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureNotificationsActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureNotificationsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Notification>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureNotificationsActions.clearCurrent());
  }
}
