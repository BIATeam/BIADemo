import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'packages/bia-ng/core/public-api';
import {
  CrudItemService,
  CrudItemSignalRService,
} from 'packages/bia-ng/shared/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
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
    private store: Store<AppState>,
    public dasService: ViewDas,
    public signalRService: CrudItemSignalRService<View>,
    protected authService: AuthService,
    public optionsService: ViewOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Plane : adapt the parent Key to the context. It can be null if root crud
    return [this.authService.getCurrentTeamId(TeamTypeId.Site)];
  }

  public getFeatureName() {
    return viewCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<View[]> = this.store.select(
    ViewsStore.getAllViews
  );
  public totalCount$: Observable<number> = this.store.select(
    ViewsStore.getViewsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    ViewsStore.getViewLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    ViewsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<View> = this.store.select(
    ViewsStore.getCurrentView
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(view => view?.name?.toString() ?? '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    ViewsStore.getViewLoadingGet
  );

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
