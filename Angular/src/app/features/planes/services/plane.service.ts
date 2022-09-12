import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { TeamTypeId } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { Plane } from '../model/plane';
import { PlaneCRUDConfiguration } from '../plane.constants';
import { FeaturePlanesStore } from '../store/plane.state';
import { FeaturePlanesActions } from '../store/planes-actions';
import { PlaneOptionsService } from './plane-options.service';
import { PlaneDas } from './plane-das.service';
import { CrudItemSignalRService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item-signalr.service';

@Injectable({
    providedIn: 'root'
})
export class PlaneService extends CrudItemService<Plane> {

    constructor(private store: Store<AppState>,
        public dasService: PlaneDas,
        public signalRService: CrudItemSignalRService<Plane>,
        public optionsService: PlaneOptionsService,
        // requiered only for parent key
        protected authService: AuthService,
        ) {
        super(dasService,signalRService,optionsService);
    }

    public getParentKey()
    {
        // TODO after CRUD creation : adapt the parent Key the the context can be null if root crud
        return this.authService.getCurrentTeamId(TeamTypeId.Site);
    }

    public getFeatureName()  {  return PlaneCRUDConfiguration.featureName; };
    public getSignalRTargetedFeature() { return {parentKey: this.getParentKey()?.toString() , featureName : this.getFeatureName()}; }


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
        // TODO after CRUD creation : map parent Key on the corresponding field
        crudItem.siteId = this.getParentKey(),
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
