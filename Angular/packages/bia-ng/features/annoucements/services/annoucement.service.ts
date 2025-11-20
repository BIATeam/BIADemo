import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { HistoricalEntryDto } from 'packages/bia-ng/models/dto/historical-entry-dto';
import {
  CrudItemService,
  CrudItemSignalRService,
} from 'packages/bia-ng/shared/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { annoucementCRUDConfiguration } from '../annoucement.constants';
import { Annoucement } from '../model/annoucement';
import { FeatureAnnoucementsStore } from '../store/annoucement.state';
import { FeatureAnnoucementsActions } from '../store/annoucements-actions';
import { AnnoucementDas } from './annoucement-das.service';
import { AnnoucementOptionsService } from './annoucement-options.service';

@Injectable({
  providedIn: 'root',
})
export class AnnoucementService extends CrudItemService<Annoucement> {
  _updateSuccessActionType = FeatureAnnoucementsActions.loadAllByPost.type;
  _createSuccessActionType = FeatureAnnoucementsActions.loadAllByPost.type;
  _updateFailureActionType = FeatureAnnoucementsActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: AnnoucementDas,
    public signalRService: CrudItemSignalRService<Annoucement>,
    public optionsService: AnnoucementOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Annoucement : adapt the parent Key to the context. It can be null if root crud
    return [];
  }

  public getFeatureName() {
    return annoucementCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Annoucement[]> = this.store.select(
    FeatureAnnoucementsStore.getAllAnnoucements
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureAnnoucementsStore.getAnnoucementsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureAnnoucementsStore.getAnnoucementLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureAnnoucementsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Annoucement> = this.store.select(
    FeatureAnnoucementsStore.getCurrentAnnoucement
  );

  public crudItemHistorical$: Observable<HistoricalEntryDto[]> =
    this.store.select(FeatureAnnoucementsStore.getCurrentAnnoucementHistorical);

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(() => '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureAnnoucementsStore.getAnnoucementLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureAnnoucementsActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureAnnoucementsActions.loadAllByPost({ event }));
  }
  public create(crudItem: Annoucement) {
    this.store.dispatch(
      FeatureAnnoucementsActions.create({ annoucement: crudItem })
    );
  }
  public update(crudItem: Annoucement) {
    this.store.dispatch(
      FeatureAnnoucementsActions.update({ annoucement: crudItem })
    );
  }
  public remove(id: any) {
    this.store.dispatch(FeatureAnnoucementsActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureAnnoucementsActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureAnnoucementsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Annoucement>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureAnnoucementsActions.clearCurrent());
  }
  public loadHistoric(id: any): void {
    this.store.dispatch(FeatureAnnoucementsActions.loadHistorical({ id: id }));
  }
}
