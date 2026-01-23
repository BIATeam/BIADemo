import { Injector } from '@angular/core';
import {
  BiaFieldsConfig,
  DataResult,
  DeleteParam,
  DeletesParam,
  GetHistoricalParam,
  GetListByPostParam,
  GetListParam,
  GetParam,
  HistoricalEntryDto,
  PostParam,
  PutParam,
  SaveParam,
  UpdateFixedStatusParam,
} from 'packages/bia-ng/models/public-api';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { GenericDas } from './generic-das.service';

export abstract class AbstractDas<
  TOutList,
  TOut = TOutList,
  TIn = Pick<TOut, Exclude<keyof TOut, 'id'>>,
> extends GenericDas {
  private localTimeFields: string[] | undefined;

  constructor(
    injector: Injector,
    endpoint: string,
    biaFieldsConfig?: BiaFieldsConfig<TOut>
  ) {
    super(injector, endpoint);
    this.setLocalTimeFields(biaFieldsConfig);
  }

  private setLocalTimeFields(biaFieldsConfig?: BiaFieldsConfig<TOut>) {
    this.localTimeFields =
      biaFieldsConfig?.columns
        .filter(field => field.asLocalDateTime === true)
        .map(field => field.field as string) ?? [];
  }

  private markLocalFiltersAsLocal(event?: TableLazyLoadEvent) {
    const localTimeFields = this.localTimeFields;
    if (!event?.filters || !localTimeFields?.length) {
      return;
    }

    Object.keys(event.filters).forEach(key => {
      if (localTimeFields.includes(key)) {
        const filter = event.filters![key];
        if (Array.isArray(filter)) {
          filter.forEach(f => {
            (f as any).isLocal = true;
          });
        } else if (filter) {
          (filter as any).isLocal = true;
        }
      }
    });
  }

  getList(param?: GetListParam): Observable<TOutList[]> {
    return this.getListItems<TOutList>(param);
  }

  getListByPost(param: GetListByPostParam): Observable<DataResult<TOutList[]>> {
    this.markLocalFiltersAsLocal(param.event);
    return this.getListItemsByPost<TOutList>(param);
  }

  get(param?: GetParam): Observable<TOut> {
    return this.getItem<TOut>(param);
  }

  save(param: SaveParam<TIn>) {
    return this.saveItem<TIn, TOut>(param);
  }

  put(param: PutParam<TIn>) {
    param.localTimeFields = this.localTimeFields;
    return this.putItem<TIn, TOut>(param);
  }

  post(param: PostParam<TIn>) {
    param.localTimeFields = this.localTimeFields;
    return this.postItem<TIn, TOut>(param);
  }

  delete(param: DeleteParam) {
    return this.deleteItem(param);
  }

  deletes(param: DeletesParam) {
    return this.deleteItems(param);
  }

  getFile(event: TableLazyLoadEvent, endpoint = 'csv'): Observable<any> {
    return this.getItemFile(event, endpoint);
  }

  updateFixedStatus(param: UpdateFixedStatusParam): Observable<TOut> {
    return this.updateFixedStatusItem<TOut>(param);
  }

  getHistorical(param: GetHistoricalParam): Observable<HistoricalEntryDto[]> {
    return this.getItemHistorical(param);
  }
}
