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
  TOutListItem,
  TOutSingleItem = TOutListItem,
  TIn = Pick<TOutSingleItem, Exclude<keyof TOutSingleItem, 'id'>>,
> extends GenericDas {
  private localTimeFields: string[] | undefined;

  constructor(
    injector: Injector,
    endpoint: string,
    biaFieldsConfig?: BiaFieldsConfig<TOutSingleItem>
  ) {
    super(injector, endpoint);
    this.setLocalTimeFields(biaFieldsConfig);
  }

  private setLocalTimeFields(
    biaFieldsConfig?: BiaFieldsConfig<TOutSingleItem>
  ) {
    this.localTimeFields =
      biaFieldsConfig?.columns
        .filter(field => field.asLocalDateTime === true)
        .map(field => field.field as string) ?? [];
  }

  getList(param?: GetListParam): Observable<TOutListItem[]> {
    return this.getListItems<TOutListItem>(param);
  }

  getListByPost(
    param: GetListByPostParam
  ): Observable<DataResult<TOutListItem[]>> {
    return this.getListItemsByPost<TOutListItem>(param);
  }

  get(param?: GetParam): Observable<TOutSingleItem> {
    return this.getItem<TOutSingleItem>(param);
  }

  save(param: SaveParam<TIn>) {
    return this.saveItem<TIn, TOutSingleItem>(param);
  }

  put(param: PutParam<TIn>) {
    param.localTimeFields = this.localTimeFields;
    return this.putItem<TIn, TOutSingleItem>(param);
  }

  post(param: PostParam<TIn>) {
    param.localTimeFields = this.localTimeFields;
    return this.postItem<TIn, TOutSingleItem>(param);
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

  updateFixedStatus(param: UpdateFixedStatusParam): Observable<TOutSingleItem> {
    return this.updateFixedStatusItem<TOutSingleItem>(param);
  }

  getHistorical(param: GetHistoricalParam): Observable<HistoricalEntryDto[]> {
    return this.getItemHistorical(param);
  }
}
