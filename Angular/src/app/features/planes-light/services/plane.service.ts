import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemDas } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-das.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { Plane } from '../model/plane';
import { FeaturePlanesStore } from '../store/plane.state';
import { FeaturePlanesActions } from '../store/planes-actions';
import { PlaneOptionsService } from './plane-options.service';

@Injectable({
    providedIn: 'root'
})
export class PlaneService extends CrudItemService<Plane> {
    constructor(private store: Store<AppState>,
        public dasService: CrudItemDas<Plane>,
        public signalRService: CrudItemSignalRService<Plane>,
        public optionsService: PlaneOptionsService,
        // requiered only for parent key at create
        protected authService: AuthService,
        ) {
        super(dasService,signalRService,optionsService);
    }

    public crudItems$: Observable<Plane[]> = this.store.select(FeaturePlanesStore.getAllPlanes);
    public totalCount$: Observable<number> = this.store.select(FeaturePlanesStore.getPlanesTotalCount);
    public loadingGetAll$: Observable<boolean> = this.store.select(FeaturePlanesStore.getPlaneLoadingGetAll);;
    public lastLazyLoadEvent$: Observable<LazyLoadEvent> = this.store.select(FeaturePlanesStore.getLastLazyLoadEvent);

    public crudItem$: Observable<Plane> = this.store.select(FeaturePlanesStore.getCurrentPlane);
    public loadingGet$: Observable<boolean> = this.store.select(FeaturePlanesStore.getPlaneLoadingGet);

    public load(id: any){
        this.store.dispatch(FeaturePlanesActions.load({ id }));
    }
    public loadAllByPost(event: LazyLoadEvent){
        this.store.dispatch(FeaturePlanesActions.loadAllByPost({ event }));
    }
    public create(crudItem: Plane){
        // Map parent Key
        crudItem.siteId = this.authService.getCurrentTeamId(TeamTypeId.Site),
        this.store.dispatch(FeaturePlanesActions.create({ plane : crudItem }));
    }
    public update(crudItem: Plane){
        this.store.dispatch(FeaturePlanesActions.update({ plane : crudItem }));
    }
    public remove(id: any){
        this.store.dispatch(FeaturePlanesActions.remove({ id }));
    }
    public multiRemove(ids: any[]){
        this.store.dispatch(FeaturePlanesActions.multiRemove({ ids }));
    }
}
