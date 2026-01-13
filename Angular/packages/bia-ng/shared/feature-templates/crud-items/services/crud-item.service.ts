import { Injectable, Injector } from '@angular/core';
import { AbstractDas, BiaMessageService } from '@bia-team/bia-ng/core';
import { BaseDto, HistoricalEntryDto } from '@bia-team/bia-ng/models';
import { TableLazyLoadEvent } from 'primeng/table';
import { EMPTY, first, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CrudItemOptionsService } from './crud-item-options.service';
import { CrudItemSignalRService } from './crud-item-signalr.service';
import { CrudItemSingleService } from './crud-item-single.service';

@Injectable({
  providedIn: 'root',
})
export abstract class CrudItemService<
  ListCrudItem extends BaseDto<string | number>,
  CrudItem extends BaseDto<string | number> = ListCrudItem,
> extends CrudItemSingleService<CrudItem> {
  constructor(
    public dasService: AbstractDas<ListCrudItem, CrudItem>,
    public signalRService: CrudItemSignalRService<ListCrudItem, CrudItem>,
    public optionsService: CrudItemOptionsService,
    protected injector: Injector
  ) {
    super(optionsService);
    this.biaMessageService = injector.get(BiaMessageService);
  }

  protected biaMessageService: BiaMessageService;

  abstract crudItems$: Observable<ListCrudItem[]>;
  abstract totalCount$: Observable<number>;
  abstract loadingGetAll$: Observable<boolean>;
  crudItemHistorical$: Observable<HistoricalEntryDto[]>;

  abstract loadAllByPost(event: TableLazyLoadEvent): void;

  public refreshList() {
    this.lastLazyLoadEvent$
      .pipe(first())
      .subscribe(event => this.loadAllByPost(event));
  }

  public getFile(event: TableLazyLoadEvent, endpoint = 'csv'): Observable<any> {
    return this.dasService.getFile(event, endpoint).pipe(
      catchError(err => {
        this.biaMessageService.showErrorHttpResponse(err);
        return EMPTY;
      })
    );
  }
}
