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
import { bannerMessageCRUDConfiguration } from '../banner-message.constants';
import { BannerMessage } from '../model/banner-message';
import { FeatureBannerMessagesStore } from '../store/banner-message.state';
import { FeatureBannerMessagesActions } from '../store/banner-messages-actions';
import { BannerMessageDas } from './banner-message-das.service';
import { BannerMessageOptionsService } from './banner-message-options.service';

@Injectable({
  providedIn: 'root',
})
export class BannerMessageService extends CrudItemService<BannerMessage> {
  _updateSuccessActionType = FeatureBannerMessagesActions.loadAllByPost.type;
  _createSuccessActionType = FeatureBannerMessagesActions.loadAllByPost.type;
  _updateFailureActionType = FeatureBannerMessagesActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: BannerMessageDas,
    public signalRService: CrudItemSignalRService<BannerMessage>,
    public optionsService: BannerMessageOptionsService,
    protected injector: Injector
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD BannerMessage : adapt the parent Key to the context. It can be null if root crud
    return [];
  }

  public getFeatureName() {
    return bannerMessageCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<BannerMessage[]> = this.store.select(
    FeatureBannerMessagesStore.getAllBannerMessages
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureBannerMessagesStore.getBannerMessagesTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureBannerMessagesStore.getBannerMessageLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureBannerMessagesStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<BannerMessage> = this.store.select(
    FeatureBannerMessagesStore.getCurrentBannerMessage
  );

  public crudItemHistorical$: Observable<HistoricalEntryDto[]> =
    this.store.select(
      FeatureBannerMessagesStore.getCurrentBannerMessageHistorical
    );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(() => '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureBannerMessagesStore.getBannerMessageLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureBannerMessagesActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureBannerMessagesActions.loadAllByPost({ event }));
  }
  public create(crudItem: BannerMessage) {
    this.store.dispatch(
      FeatureBannerMessagesActions.create({ bannerMessage: crudItem })
    );
  }
  public update(crudItem: BannerMessage) {
    this.store.dispatch(
      FeatureBannerMessagesActions.update({ bannerMessage: crudItem })
    );
  }
  public remove(id: any) {
    this.store.dispatch(FeatureBannerMessagesActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureBannerMessagesActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureBannerMessagesActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <BannerMessage>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureBannerMessagesActions.clearCurrent());
  }
  public loadHistoric(id: any): void {
    this.store.dispatch(
      FeatureBannerMessagesActions.loadHistorical({ id: id })
    );
  }
}
