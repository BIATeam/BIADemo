import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import {
  DeleteParam,
  DeletesParam,
  GenericDas,
  GetListByPostParam,
  GetListParam,
  GetParam,
  PostParam,
  PutParam,
  SaveParam,
} from './generic-das.service';
import { Injector } from '@angular/core';

export abstract class AbstractDas<
  TOut,
  TIn = Pick<TOut, Exclude<keyof TOut, 'id'>>,
> extends GenericDas {
  constructor(injector: Injector, endpoint: string) {
    super(injector, endpoint);
  }

  public static buildRoute(endpoint: string): string {
    return GenericDas.buildRoute(endpoint);
  }

  getList(param?: GetListParam): Observable<TOut[]> {
    return this.getListItems<TOut>(param);
  }

  get(param?: GetParam): Observable<TOut> {
    return this.getItem<TOut>(param);
  }

  getListByPost(param: GetListByPostParam): Observable<DataResult<TOut[]>> {
    return this.getListItemsByPost<TOut>(param);
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

  getFile(event: LazyLoadEvent, endpoint = 'csv'): Observable<any> {
    return this.getItemFile(event, endpoint);
  }
}
