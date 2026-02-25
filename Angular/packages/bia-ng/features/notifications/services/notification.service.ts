import { Injectable, Injector } from '@angular/core';
import { AuthService } from '@bia-team/bia-ng/core';
import { BiaTeamTypeId } from '@bia-team/bia-ng/models/enum';
import { CrudItemService } from '@bia-team/bia-ng/shared';
import { BiaAppState } from '@bia-team/bia-ng/store';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
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
    protected store: Store<BiaAppState>,
    public dasService: NotificationDas,
    public signalRService: NotificationsSignalRService,
    public optionsService: NotificationOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
    this.crudItems$ = this.store.select(
      FeatureNotificationsStore.getAllNotifications
    );
    this.totalCount$ = this.store.select(
      FeatureNotificationsStore.getNotificationsTotalCount
    );
    this.loadingGetAll$ = this.store.select(
      FeatureNotificationsStore.getNotificationLoadingGetAll
    );
    this.lastLazyLoadEvent$ = this.store.select(
      FeatureNotificationsStore.getLastLazyLoadEvent
    );
    this.crudItem$ = this.store.select(
      FeatureNotificationsStore.getCurrentNotification
    );
    this.displayItemName$ = this.crudItem$.pipe(
      map(notification => notification?.title?.toString() ?? '')
    );
    this.loadingGet$ = this.store.select(
      FeatureNotificationsStore.getNotificationLoadingGet
    );
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Notification : adapt the parent Key tothe context. It can be null if root crud
    return [this.authService.getCurrentTeamId(BiaTeamTypeId.Site)];
  }

  public getFeatureName() {
    return notificationCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<NotificationListItem[]>;
  public totalCount$: Observable<number>;
  public loadingGetAll$: Observable<boolean>;
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent>;
  public crudItem$: Observable<Notification>;
  public loadingGet$: Observable<boolean>;

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
