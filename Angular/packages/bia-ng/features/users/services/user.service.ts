import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'packages/bia-ng/core/public-api';
import {
  CrudItemService,
  CrudItemSignalRService,
} from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { User } from '../model/user';
import { FeatureUsersStore } from '../store/user.state';
import { FeatureUsersActions } from '../store/users-actions';
import { userCRUDConfiguration } from '../user.constants';
import { UserDas } from './user-das.service';
import { UserOptionsService } from './user-options.service';

@Injectable({
  providedIn: 'root',
})
export class UserService extends CrudItemService<User> {
  _updateSuccessActionType = FeatureUsersActions.loadAllByPost.type;
  _createSuccessActionType = FeatureUsersActions.loadAllByPost.type;
  _updateFailureActionType = FeatureUsersActions.failure.type;

  constructor(
    protected store: Store<BiaAppState>,
    public dasService: UserDas,
    public signalRService: CrudItemSignalRService<User>,
    public optionsService: UserOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
    this.crudItems$ = this.store.select(FeatureUsersStore.getAllUsers);
    this.totalCount$ = this.store.select(FeatureUsersStore.getUsersTotalCount);
    this.loadingGetAll$ = this.store.select(
      FeatureUsersStore.getUserLoadingGetAll
    );
    this.lastLazyLoadEvent$ = this.store.select(
      FeatureUsersStore.getLastLazyLoadEvent
    );
    this.crudItem$ = this.store.select(FeatureUsersStore.getCurrentUser);
    this.displayItemName$ = this.crudItem$.pipe(map(user => user?.displayName));
    this.loadingGet$ = this.store.select(FeatureUsersStore.getUserLoadingGet);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD User : adapt the parent Key tothe context. It can be null if root crud
    // return this.authService.getCurrentTeamId(TeamTypeId.Site);
    return [];
  }

  public getFeatureName() {
    return userCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<User[]>;
  public totalCount$: Observable<number>;
  public loadingGetAll$: Observable<boolean>;
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent>;
  public crudItem$: Observable<User>;
  public loadingGet$: Observable<boolean>;

  public load(id: any) {
    this.store.dispatch(FeatureUsersActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureUsersActions.loadAllByPost({ event }));
  }
  public create(crudItem: User) {
    // TODO after creation of CRUD User : map parent Key on the corresponding field
    // crudItem.siteId = this.getParentIds()[0],
    this.store.dispatch(FeatureUsersActions.create({ user: crudItem }));
  }
  public update(crudItem: User) {
    this.store.dispatch(FeatureUsersActions.update({ user: crudItem }));
  }
  public save(crudItems: User[]) {
    this.store.dispatch(FeatureUsersActions.save({ users: crudItems }));
  }
  public remove(id: any) {
    this.store.dispatch(FeatureUsersActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureUsersActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureUsersActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <User>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureUsersActions.clearCurrent());
  }
}
