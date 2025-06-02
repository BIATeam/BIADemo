import { Injectable } from '@angular/core';
import { TableLazyLoadEvent } from 'primeng/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { DtoState } from '../../../model/dto-state.enum';
import { BaseDto } from '../../../model/dto/base-dto';
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

  /**
   * Type of store action called after update effect is successful.
   * See update effect in store effects of CRUD item.
   */
  protected _updateSuccessActionType: string | undefined;
  public get updateSuccessActionType(): string | undefined {
    return this._updateSuccessActionType;
  }

  /**
   * Type of store action called when update effect has failed.
   * See update effect in store effects of CRUD item.
   */
  protected _updateFailureActionType: string | undefined;
  public get updateFailureActionType(): string | undefined {
    return this._updateFailureActionType;
  }

  /**
   * Type of store action called after create effect is successful.
   * See create effect in store effects of CRUD item.
   */
  protected _createSuccessActionType: string | undefined;
  public get createSuccessActionType(): string | undefined {
    return this._createSuccessActionType;
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
  displayItemName$: Observable<string> = of('');
  abstract loadingGet$: Observable<boolean>;

  abstract load(id: any): void;
  abstract create(crudItem: CrudItem): void;
  abstract update(crudItem: CrudItem): void;
  abstract remove(id: any): void;
  abstract multiRemove(ids: any[]): void;
  abstract clearAll(): void;
  abstract clearCurrent(): void;
  // eslint-disable-next-line @typescript-eslint/no-empty-function, @typescript-eslint/no-unused-vars
  updateFixedStatus(id: any, isFixed: boolean): void {}

  protected resetNewItemsIds(dtos: BaseDto[] | undefined): void {
    dtos?.forEach(dto => {
      if (dto.id < 0) {
        dto.id = 0;
        dto.dtoState = DtoState.Added;
      }
    });
  }

  protected configChanged: BehaviorSubject<void> = new BehaviorSubject<void>(
    undefined
  );
  configChanged$: Observable<void> = this.configChanged.asObservable();

  notifyConfigChange(): void {
    this.configChanged.next();
  }
}
