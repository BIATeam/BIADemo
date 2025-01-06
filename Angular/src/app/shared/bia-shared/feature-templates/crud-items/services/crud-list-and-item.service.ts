import { Injectable } from '@angular/core';
import { TableLazyLoadEvent } from 'primeng/table';
import { first, Observable } from 'rxjs';
import { AbstractDasWithListAndItem } from 'src/app/core/bia-core/services/abstract-das-with-list-and-item.service';
import { BaseDto } from '../../../model/base-dto';
import { TargetedFeature } from '../../../model/signalR';
import { CrudItemOptionsService } from './crud-item-options.service';
import { CrudItemSignalRService } from './crud-item-signalr.service';
import { CrudItemSingleService } from './crud-item-single.service';

@Injectable({
  providedIn: 'root',
})
export abstract class CrudListAndItemService<
  CrudItem extends BaseDto,
  CrudItemList extends BaseDto,
> extends CrudItemSingleService<CrudItem> {
  public siganlRTargetedFeature: TargetedFeature;

  constructor(
    public dasService: AbstractDasWithListAndItem<CrudItem, CrudItemList>,
    public signalRService: CrudItemSignalRService<CrudItemList>,
    public optionsService: CrudItemOptionsService
  ) {
    super(optionsService);
  }

  abstract crudItems$: Observable<CrudItemList[]>;
  abstract totalCount$: Observable<number>;
  abstract loadingGetAll$: Observable<boolean>;

  abstract loadAllByPost(event: TableLazyLoadEvent): void;

  public refreshList() {
    this.lastLazyLoadEvent$
      .pipe(first())
      .subscribe(event => this.loadAllByPost(event));
  }
}
