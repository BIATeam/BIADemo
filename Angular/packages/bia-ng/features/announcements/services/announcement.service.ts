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
import { announcementCRUDConfiguration } from '../announcement.constants';
import { Announcement } from '../model/announcement';
import { FeatureAnnouncementsStore } from '../store/announcement.state';
import { FeatureAnnouncementsActions } from '../store/announcements-actions';
import { AnnouncementDas } from './announcement-das.service';
import { AnnouncementOptionsService } from './announcement-options.service';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementService extends CrudItemService<Announcement> {
  _updateSuccessActionType = FeatureAnnouncementsActions.loadAllByPost.type;
  _createSuccessActionType = FeatureAnnouncementsActions.loadAllByPost.type;
  _updateFailureActionType = FeatureAnnouncementsActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: AnnouncementDas,
    public signalRService: CrudItemSignalRService<Announcement>,
    public optionsService: AnnouncementOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Announcement : adapt the parent Key to the context. It can be null if root crud
    return [];
  }

  public getFeatureName() {
    return announcementCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Announcement[]> = this.store.select(
    FeatureAnnouncementsStore.getAllAnnouncements
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureAnnouncementsStore.getAnnouncementsTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureAnnouncementsStore.getAnnouncementLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureAnnouncementsStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Announcement> = this.store.select(
    FeatureAnnouncementsStore.getCurrentAnnouncement
  );

  public crudItemHistorical$: Observable<HistoricalEntryDto[]> =
    this.store.select(FeatureAnnouncementsStore.getCurrentAnnouncementHistorical);

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(() => '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureAnnouncementsStore.getAnnouncementLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureAnnouncementsActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureAnnouncementsActions.loadAllByPost({ event }));
  }
  public create(crudItem: Announcement) {
    this.store.dispatch(
      FeatureAnnouncementsActions.create({ announcement: crudItem })
    );
  }
  public update(crudItem: Announcement) {
    this.store.dispatch(
      FeatureAnnouncementsActions.update({ announcement: crudItem })
    );
  }
  public remove(id: any) {
    this.store.dispatch(FeatureAnnouncementsActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureAnnouncementsActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureAnnouncementsActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Announcement>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureAnnouncementsActions.clearCurrent());
  }
  public loadHistoric(id: any): void {
    this.store.dispatch(FeatureAnnouncementsActions.loadHistorical({ id: id }));
  }
}
