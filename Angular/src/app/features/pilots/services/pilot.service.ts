import { Injectable, Injector } from '@angular/core';
import { AuthService } from '@bia-team/bia-ng/core';
import {
  CrudItemService,
  CrudItemSignalRService,
} from '@bia-team/bia-ng/shared';
import { Store } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { Pilot } from '../model/pilot';
import { pilotCRUDConfiguration } from '../pilot.constants';
import { FeaturePilotsStore } from '../store/pilot.state';
import { FeaturePilotsActions } from '../store/pilots-actions';
import { PilotDas } from './pilot-das.service';
import { PilotOptionsService } from './pilot-options.service';

@Injectable({
  providedIn: 'root',
})
export class PilotService extends CrudItemService<Pilot> {
  _updateSuccessActionType = FeaturePilotsActions.loadAllByPost.type;
  _createSuccessActionType = FeaturePilotsActions.loadAllByPost.type;
  _updateFailureActionType = FeaturePilotsActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: PilotDas,
    public signalRService: CrudItemSignalRService<Pilot>,
    protected authService: AuthService,
    public optionsService: PilotOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Pilot : adapt the parent Key to the context. It can be null if root crud
    return [this.authService.getCurrentTeamId(TeamTypeId.Site)];
  }

  public getFeatureName() {
    return pilotCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Pilot[]> = this.store.select(
    FeaturePilotsStore.getAllPilots
  );
  public totalCount$: Observable<number> = this.store.select(
    FeaturePilotsStore.getPilotsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeaturePilotsStore.getPilotLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeaturePilotsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Pilot> = this.store.select(
    FeaturePilotsStore.getCurrentPilot
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(pilot => pilot?.identificationNumber?.toString() ?? '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeaturePilotsStore.getPilotLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeaturePilotsActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeaturePilotsActions.loadAllByPost({ event }));
  }
  public create(crudItem: Pilot) {
    crudItem.siteId = this.getParentIds()[0];
    this.store.dispatch(FeaturePilotsActions.create({ pilot: crudItem }));
  }
  public save(crudItems: Pilot[]) {
    crudItems.map(x => (x.siteId = this.getParentIds()[0]));
    this.store.dispatch(FeaturePilotsActions.save({ pilots: crudItems }));
  }
  public update(crudItem: Pilot) {
    this.store.dispatch(FeaturePilotsActions.update({ pilot: crudItem }));
  }
  public remove(id: any) {
    this.store.dispatch(FeaturePilotsActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeaturePilotsActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeaturePilotsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Pilot>{ id: '' };
    this._currentCrudItemId = '';
    this.store.dispatch(FeaturePilotsActions.clearCurrent());
  }
  public updateFixedStatus(id: any, isFixed: boolean): void {
    this.store.dispatch(
      FeaturePilotsActions.updateFixedStatus({ id: id, isFixed: isFixed })
    );
  }
}
