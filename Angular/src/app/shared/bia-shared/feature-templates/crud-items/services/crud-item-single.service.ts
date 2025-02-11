import { Injectable } from '@angular/core';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { BaseDto } from '../../../model/base-dto';
import { DtoState } from '../../../model/dto-state.enum';
import { CrudItemOptionsService } from './crud-item-options.service';

@Injectable({
  providedIn: 'root',
})
export abstract class CrudItemSingleService<CrudItem extends BaseDto> {
  constructor(public optionsService: CrudItemOptionsService) {
    setTimeout(() => this.initSub()); // should be done after initialization of the parent constructor
  }
  protected capitalizeFirstLetter(string: string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
  }

  public getParentIds(): any[] {
    return [];
  }

  public getParentKey(): string {
    return this.getParentIds()
      .map(id => id.toString())
      .join('-');
  }

  public getFeatureName() {
    return 'crud-items';
  }
  public getSignalRTargetedFeature() {
    return {
      parentKey: this.getParentKey(),
      featureName: this.getFeatureName(),
    };
  }

  public getConsoleLabel() {
    return this.capitalizeFirstLetter(
      this.getFeatureName().replace(/-./g, x => x[1].toUpperCase())
    );
  }

  public getSignalRRefreshEvent() {
    return 'refresh-' + this.getFeatureName();
  }

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
    this.load(id);
  }

  initSub() {
    this.crudItem$.subscribe(crudItem => {
      if (crudItem) {
        this._currentCrudItem = crudItem;
      }
    });
  }

  abstract lastLazyLoadEvent$: Observable<TableLazyLoadEvent>;

  abstract crudItem$: Observable<CrudItem>;
  abstract loadingGet$: Observable<boolean>;

  abstract load(id: any): void;
  abstract create(crudItem: CrudItem): void;
  abstract update(crudItem: CrudItem): void;
  abstract remove(id: any): void;
  abstract multiRemove(ids: any[]): void;
  abstract clearAll(): void;
  abstract clearCurrent(): void;

  protected resetNewItemsIds(dtos: BaseDto[] | undefined): void {
    dtos?.forEach(dto => {
      if (dto.id < 0) {
        dto.id = 0;
        dto.dtoState = DtoState.Added;
      }
    });
  }
}
