import { Injectable } from '@angular/core';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { BaseDto } from '../../../model/base-dto';
import { TargetedFeature } from '../../../model/signalR';
import { CrudItemOptionsService } from './crud-item-options.service';
import { CrudItemSignalRService } from './crud-item-signalr.service';

@Injectable({
    providedIn: 'root'
})
export abstract class CrudItemService<CrudItem extends BaseDto> {
    public siganlRTargetedFeature: TargetedFeature;
    
    constructor(    
        public dasService: AbstractDas<CrudItem>,
        public signalRService: CrudItemSignalRService<CrudItem>,
        public optionsService: CrudItemOptionsService,
    )
    {
        setTimeout(() => this.InitSub()); // should be done after initialization of the parent constructor
    }
    protected capitalizeFirstLetter(string : string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
      }

    public getParentIds(): any[]
    {
         return [];
    } 

    public getParentKey(): string
    {
        return this.getParentIds().map((id) => id.toString()).join("-");
    }

    public getFeatureName()  {  return 'crud-items'; };
    public getSignalRTargetedFeature() { return {parentKey: this.getParentKey() , featureName : this.getFeatureName()}; }

    public getConsoleLabel()  {  return this.capitalizeFirstLetter(this.getFeatureName().replace(/-./g, x=>x[1].toUpperCase())); };

    public getSignalRRefreshEvent() { return 'refresh-' + this.getFeatureName(); };


    protected _currentCrudItem: CrudItem;
    protected _currentCrudItemId: any;

    public get currentCrudItem() {
        if (this._currentCrudItem?.id === this._currentCrudItemId) {
            return this._currentCrudItem;
        } else {
            return null;
        }
    }

    public get currentCrudItemId(): any {
        return this._currentCrudItemId;
    }
    public set currentCrudItemId(id: any) {
        this._currentCrudItemId = id;
        this.load( id );
    }

    InitSub() {
        this.crudItem$.subscribe((crudItem) => {
            if (crudItem) {
                this._currentCrudItem = crudItem;
            }
        })
    }


    abstract crudItems$: Observable<CrudItem[]>;
    abstract totalCount$: Observable<number>;
    abstract loadingGetAll$: Observable<boolean>;
    abstract lastLazyLoadEvent$: Observable<LazyLoadEvent>;

    abstract crudItem$: Observable<CrudItem>;
    abstract loadingGet$: Observable<boolean>;

    abstract load(id: any): void;
    abstract loadAllByPost(event: LazyLoadEvent): void;
    abstract create(crudItem: CrudItem):void;
    abstract update(crudItem: CrudItem):void;
    abstract remove(id: any):void;
    abstract multiRemove(ids: any[]):void;
}
