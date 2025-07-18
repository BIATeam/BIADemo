import { Injector } from '@angular/core';
import {
  DataResult,
  DeleteParam,
  DeletesParam,
  GetListByPostParam,
  GetListParam,
  GetParam,
  PostParam,
  PutParam,
  SaveParam,
  UpdateFixedStatusParam,
} from 'biang/models';
import { TableLazyLoadEvent } from 'primeng/table';
import { Observable } from 'rxjs';
import { GenericDas } from './generic-das.service';

export abstract class AbstractDas<
  TOutList,
  TOut = TOutList,
  TIn = Pick<TOut, Exclude<keyof TOut, 'id'>>,
> extends GenericDas {
  constructor(injector: Injector, endpoint: string) {
    super(injector, endpoint);
  }

  getList(param?: GetListParam): Observable<TOutList[]> {
    return this.getListItems<TOutList>(param);
  }

  getListByPost(param: GetListByPostParam): Observable<DataResult<TOutList[]>> {
    return this.getListItemsByPost<TOutList>(param);
  }

  get(param?: GetParam): Observable<TOut> {
    return this.getItem<TOut>(param);
  }

  save(param: SaveParam<TIn>) {
    return this.saveItem<TIn, TOut>(param);
  }

  put(param: PutParam<TIn>) {
    return this.putItem<TIn, TOut>(param);
  }

  post(param: PostParam<TIn>) {
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
}
