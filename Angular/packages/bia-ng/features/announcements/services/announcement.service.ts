import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { Announcement } from 'packages/bia-ng/models/public-api';
import {
  CrudItemService,
  CrudItemSignalRService,
} from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { announcementCRUDConfiguration } from '../announcement.constants';
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
    private store: Store<BiaAppState>,
    public dasService: AnnouncementDas,
    public signalRService: CrudItemSignalRService<Announcement>,
    public optionsService: AnnouncementOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
    this.crudItems$ = this.store.select(
      FeatureAnnouncementsStore.getAllAnnouncements
    );
    this.totalCount$ = this.store.select(
      FeatureAnnouncementsStore.getAnnouncementsTotalCount
    );
    this.loadingGetAll$ = this.store.select(
      FeatureAnnouncementsStore.getAnnouncementLoadingGetAll
    );
    this.lastLazyLoadEvent$ = this.store.select(
      FeatureAnnouncementsStore.getLastLazyLoadEvent
    );
    this.crudItem$ = this.store.select(
      FeatureAnnouncementsStore.getCurrentAnnouncement
    );
    this.crudItemHistorical$ = this.store.select(
      FeatureAnnouncementsStore.getCurrentAnnouncementHistorical
    );
    this.displayItemName$ = this.crudItem$.pipe(map(() => ''));
    this.loadingGet$ = this.store.select(
      FeatureAnnouncementsStore.getAnnouncementLoadingGet
    );
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Announcement : adapt the parent Key to the context. It can be null if root crud
    return [];
  }

  public getFeatureName() {
    return announcementCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Announcement[]>;
  public totalCount$: Observable<number>;
  public loadingGetAll$: Observable<boolean>;
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent>;
  public crudItem$: Observable<Announcement>;
  public loadingGet$: Observable<boolean>;

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
