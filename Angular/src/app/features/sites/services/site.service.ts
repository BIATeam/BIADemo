import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { AppState } from 'src/app/store/state';
import { Site } from '../model/site';
import { siteCRUDConfiguration } from '../site.constants';
import { FeatureSitesStore } from '../store/site.state';
import { FeatureSitesActions } from '../store/sites-actions';
import { SiteOptionsService } from './site-options.service';
import { SiteDas } from './site-das.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { TeamTypeId } from 'src/app/shared/constants';

@Injectable({
  providedIn: 'root',
})
export class SiteService extends CrudItemService<Site> {
  constructor(
    private store: Store<AppState>,
    public dasService: SiteDas,
    public signalRService: CrudItemSignalRService<Site>,
    public optionsService: SiteOptionsService,
    // requiered only for parent key
    protected authService: AuthService
  ) {
    super(dasService, signalRService, optionsService);
  }

  // Custo for teams
  public get currentCrudItemId(): any {
    // should be redifine due to the setter
    return super.currentCrudItemId;
  }

  // Custo for teams
  public set currentCrudItemId(id: any) {
    if (this._currentCrudItemId !== id) {
      this._currentCrudItemId = id;
      this.authService.changeCurrentTeamId(TeamTypeId.Site, id);
    }
    this.load(id);
  }

  public getParentIds(): any[] {
    // TODO after creation of CRUD Team Site : adapt the parent Key tothe context. It can be null if root crud
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
  public lastLazyLoadEvent$: Observable<LazyLoadEvent> = this.store.select(
    FeatureSitesStore.getLastLazyLoadEvent
  );

  public crudItem$: Observable<Site> = this.store.select(
    FeatureSitesStore.getCurrentSite
  );
  public loadingGet$: Observable<boolean> = this.store.select(
    FeatureSitesStore.getSiteLoadingGet
  );

  public load(id: any) {
    this.store.dispatch(FeatureSitesActions.load({ id }));
  }
  public loadAllByPost(event: LazyLoadEvent) {
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
