import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'packages/bia-ng/core/public-api';
import { BiaTeamTypeId } from 'packages/bia-ng/models/enum/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { CrudItemSignalRService } from '../../../feature-templates/crud-items/services/crud-item-signalr.service';
import { CrudItemService } from '../../../feature-templates/crud-items/services/crud-item.service';
import { View } from '../model/view';
import { viewCRUDConfiguration } from '../model/view.constants';
import { ViewsStore } from '../store/view.state';
import { ViewsActions } from '../store/views-actions';
import { ViewDas } from './view-das.service';
import { ViewOptionsService } from './view-options.service';

@Injectable({
  providedIn: 'root',
})
export class ViewService extends CrudItemService<View> {
  _updateSuccessActionType = ViewsActions.loadAllView.type;
  _createSuccessActionType = ViewsActions.loadAllView.type;
  _updateFailureActionType = ViewsActions.failure.type;

  constructor(
    private store: Store<BiaAppState>,
    public dasService: ViewDas,
    public signalRService: CrudItemSignalRService<View>,
    protected authService: AuthService,
    public optionsService: ViewOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
    this.crudItems$ = this.store.select(ViewsStore.getAllViews);
    this.totalCount$ = this.store.select(ViewsStore.getViewsTotalCount);
    this.loadingGetAll$ = this.store.select(ViewsStore.getViewLoadingGetAll);
    this.lastLazyLoadEvent$ = this.store.select(
      ViewsStore.getLastLazyLoadEvent
    );
    this.crudItem$ = this.store.select(ViewsStore.getCurrentView);
    this.displayItemName$ = this.crudItem$.pipe(
      map(view => view?.name?.toString() ?? '')
    );
    this.loadingGet$ = this.store.select(ViewsStore.getViewLoadingGet);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Plane : adapt the parent Key to the context. It can be null if root crud
    return [this.authService.getCurrentTeamId(BiaTeamTypeId.Site)];
  }

  public getFeatureName() {
    return viewCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<View[]>;
  public totalCount$: Observable<number>;
  public loadingGetAll$: Observable<boolean>;
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent>;
  public crudItem$: Observable<View>;
  public loadingGet$: Observable<boolean>;

  public load(id: any) {
    if (id === '0' || id === 0) {
      this.store.dispatch(ViewsActions.clearCurrent());
    } else {
      this.store.dispatch(ViewsActions.load({ id }));
    }
  }
  public loadAllByPost() {
    this.store.dispatch(ViewsActions.loadAllView());
  }
  public create(crudItem: View) {
    if (crudItem.viewTeams?.length) {
      this.store.dispatch(ViewsActions.addTeamView(crudItem));
    } else {
      this.store.dispatch(ViewsActions.addUserView(crudItem));
    }
  }
  public update(crudItem: View) {
    if (crudItem.id === 0) {
      this.create(crudItem);
    } else {
      if (crudItem.viewTeams?.length) {
        this.store.dispatch(ViewsActions.updateTeamView(crudItem));
      } else {
        this.store.dispatch(ViewsActions.updateUserView(crudItem));
      }
    }
  }
  public remove(id: any) {
    this.store.dispatch(ViewsActions.removeTeamView({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(ViewsActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(ViewsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <View>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(ViewsActions.clearCurrent());
  }
}
