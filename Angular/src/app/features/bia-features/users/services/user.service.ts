import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
import { User } from '../model/user';
import { userCRUDConfiguration } from '../user.constants';
import { FeatureUsersStore } from '../store/user.state';
import { FeatureUsersActions } from '../store/users-actions';
import { UserOptionsService } from './user-options.service';
import { UserDas } from './user-das.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';

@Injectable({
  providedIn: 'root',
})
export class UserService extends CrudItemService<User> {
  constructor(
    protected store: Store<AppState>,
    public dasService: UserDas,
    public signalRService: CrudItemSignalRService<User>,
    public optionsService: UserOptionsService,
    // requiered only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService);
  }

  getParentIds(): any[] {
    // TODO after creation of CRUD User : adapt the parent Key tothe context. It can be null if root crud
    // return this.authService.getCurrentTeamId(TeamTypeId.Site);
    return [];
  }

  getFeatureName() {
    return userCRUDConfiguration.featureName;
  }

  crudItems$: Observable<User[]> = this.store.select(
    FeatureUsersStore.getAllUsers
  );
  totalCount$: Observable<number> = this.store.select(
    FeatureUsersStore.getUsersTotalCount
  );
  loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureUsersStore.getUserLoadingGetAll
  );
  lastLazyLoadEvent$: Observable<LazyLoadEvent> = this.store.select(
    FeatureUsersStore.getLastLazyLoadEvent
  );

  crudItem$: Observable<User> = this.store.select(
    FeatureUsersStore.getCurrentUser
  );
  loadingGet$: Observable<boolean> = this.store.select(
    FeatureUsersStore.getUserLoadingGet
  );

  load(id: any) {
    this.store.dispatch(FeatureUsersActions.load({ id }));
  }
  loadAllByPost(event: LazyLoadEvent) {
    this.store.dispatch(FeatureUsersActions.loadAllByPost({ event }));
  }
  create(crudItem: User) {
    // TODO after creation of CRUD User : map parent Key on the corresponding field
    // crudItem.siteId = this.getParentIds()[0],
    this.store.dispatch(FeatureUsersActions.create({ user: crudItem }));
  }
  update(crudItem: User) {
    this.store.dispatch(FeatureUsersActions.update({ user: crudItem }));
  }
  save(crudItems: User[]) {
    this.store.dispatch(FeatureUsersActions.save({ users: crudItems }));
  }
  remove(id: any) {
    this.store.dispatch(FeatureUsersActions.remove({ id }));
  }
  multiRemove(ids: any[]) {
    this.store.dispatch(FeatureUsersActions.multiRemove({ ids }));
  }
  clearAll() {
    this.store.dispatch(FeatureUsersActions.clearAll());
  }
  clearCurrent() {
    this._currentCrudItem = <User>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureUsersActions.clearCurrent());
  }
}
