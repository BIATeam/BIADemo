import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
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
  constructor(
    protected store: Store<AppState>,
    public dasService: UserDas,
    public signalRService: CrudItemSignalRService<User>,
    public optionsService: UserOptionsService,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD User : adapt the parent Key tothe context. It can be null if root crud
    // return this.authService.getCurrentTeamId(TeamTypeId.Site);
    return [];
  }

  public getFeatureName() {
    return userCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<User[]> = this.store.select(
    FeatureUsersStore.getAllUsers
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureUsersStore.getUsersTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureUsersStore.getUserLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureUsersStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<User> = this.store.select(
    FeatureUsersStore.getCurrentUser
  );
  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureUsersStore.getUserLoadingGet
  );

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
