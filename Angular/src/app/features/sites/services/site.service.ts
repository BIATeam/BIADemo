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
import { Site } from '../model/site';
import { siteCRUDConfiguration } from '../site.constants';
import { FeatureSitesStore } from '../store/site.state';
import { FeatureSitesActions } from '../store/sites-actions';
import { SiteDas } from './site-das.service';
import { SiteOptionsService } from './site-options.service';

@Injectable({
  providedIn: 'root',
})
export class SiteService extends CrudItemService<Site> {
  _updateSuccessActionType = FeatureSitesActions.loadAllByPost.type;
  _createSuccessActionType = FeatureSitesActions.loadAllByPost.type;
  _updateFailureActionType = FeatureSitesActions.failure.type;

  constructor(
    private store: Store<AppState>,
    public dasService: SiteDas,
    public signalRService: CrudItemSignalRService<Site>,
    public optionsService: SiteOptionsService,
    protected injector: Injector,
    // required only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService, injector);
  }

  // Customization for teams
  public get currentCrudItemId(): any {
    // should be redefine due to the setter
    return super.currentCrudItemId;
  }

  // Customization for teams
  public set currentCrudItemId(id: any) {
    if (this._currentCrudItemId !== id) {
      this._currentCrudItemId = id;
      this.authService.changeCurrentTeamId(TeamTypeId.Site, id);
    }
    this.load(id);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Team Site : adapt the parent Key to the context. It can be null if root crud
    //return this.authService.getCurrentTeamId(TeamTypeId.Site);
    return [];
  }

  public getFeatureName() {
    return siteCRUDConfiguration.featureName;
  }

  public crudItems$: Observable<Site[]> = this.store.select(
    FeatureSitesStore.getAllSites
  );
  public totalCount$: Observable<number> = this.store.select(
    FeatureSitesStore.getSitesTotalCount
  );
  public loadingGetAll$: Observable<boolean> = this.store.select(
    FeatureSitesStore.getSiteLoadingGetAll
  );
  public lastLazyLoadEvent$: Observable<TableLazyLoadEvent> = this.store.select(
    FeatureSitesStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Site> = this.store.select(
    FeatureSitesStore.getCurrentSite
  );

  public displayItemName$: Observable<string> = this.crudItem$.pipe(
    map(site => site?.title?.toString() ?? '')
  );

  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureSitesStore.getSiteLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureSitesActions.load({ id }));
  }
  public loadAllByPost(event: TableLazyLoadEvent) {
    this.store.dispatch(FeatureSitesActions.loadAllByPost({ event }));
  }
  public create(crudItem: Site) {
    // TODO after creation of CRUD Team Site : map parent Key on the corresponding field
    // crudItem.siteId = this.getParentIds()[0],
    this.store.dispatch(FeatureSitesActions.create({ site: crudItem }));
  }
  public update(crudItem: Site) {
    this.store.dispatch(FeatureSitesActions.update({ site: crudItem }));
  }
  public remove(id: any) {
    this.store.dispatch(FeatureSitesActions.remove({ id }));
  }
  public multiRemove(ids: any[]) {
    this.store.dispatch(FeatureSitesActions.multiRemove({ ids }));
  }
  public clearAll() {
    this.store.dispatch(FeatureSitesActions.clearAll());
  }
  public clearCurrent() {
    this._currentCrudItem = <Site>{};
    this._currentCrudItemId = 0;
    this.store.dispatch(FeatureSitesActions.clearCurrent());
  }
}
