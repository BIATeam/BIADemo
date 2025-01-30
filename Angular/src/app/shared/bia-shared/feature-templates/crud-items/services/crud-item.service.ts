import { Injectable } from '@angular/core';
import { TableLazyLoadEvent } from 'primeng/table';
import { first, Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { BaseDto } from '../../../model/base-dto';
import { TargetedFeature } from '../../../model/signalR';
import { CrudItemOptionsService } from './crud-item-options.service';
import { CrudItemSignalRService } from './crud-item-signalr.service';
import { CrudItemSingleService } from './crud-item-single.service';

@Injectable({
  providedIn: 'root',
})
export abstract class CrudItemService<
  ListCrudItem extends BaseDto,
  CrudItem extends BaseDto = ListCrudItem,
> extends CrudItemSingleService<CrudItem> {
  public siganlRTargetedFeature: TargetedFeature;

  constructor(
    public dasService: AbstractDas<ListCrudItem, CrudItem>,
    public signalRService: CrudItemSignalRService<ListCrudItem, CrudItem>,
    public optionsService: CrudItemOptionsService
  ) {
    super(optionsService);
  }

  abstract crudItems$: Observable<ListCrudItem[]>;
  abstract totalCount$: Observable<number>;
  abstract loadingGetAll$: Observable<boolean>;

  abstract loadAllByPost(event: TableLazyLoadEvent): void;

  public refreshList() {
    this.lastLazyLoadEvent$
      .pipe(first())
      .subscribe(event => this.loadAllByPost(event));
  }
}
