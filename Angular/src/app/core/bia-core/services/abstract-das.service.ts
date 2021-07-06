import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { GenericDas, HttpOptions } from './generic-das.service';
import { Injector } from '@angular/core';

export abstract class AbstractDas<TOut, TIn = Pick<TOut, Exclude<keyof TOut, 'id'>>> extends GenericDas {

  constructor(injector: Injector, endpoint: string) {
    super(injector, endpoint);
  }

  public static buildRoute(endpoint: string): string {
    return GenericDas.buildRoute(endpoint);
  }

  get(id: string | number, options?: HttpOptions): Observable<TOut> {
    return this.getItem<TOut>(id, options);
  }

  getList(endpoint: string = '', options?: HttpOptions): Observable<TOut[]> {
    return this.getListItems<TOut>(endpoint, options);
  }

  getListByPost(event: LazyLoadEvent, endpoint: string = 'all'): Observable<DataResult<TOut[]>> {
    return this.getListItemsByPost<TOut>(event, endpoint);
  }

  save(items: TIn[], endpoint: string = 'save', options?: HttpOptions) {
    return this.saveItem<TIn, TOut>(items, endpoint, options);
  }

  put(item: TIn, id: string | number, options?: HttpOptions) {
    return this.putItem<TIn, TOut>(item, id, options);
  }

  post(item: TIn, options?: HttpOptions) {
    return this.postItem<TIn, TOut>(item, options);
  }

  delete(id: string | number, options?: HttpOptions) {
    return this.deleteItem(id, options);
  }

  deletes(ids: string[] | number[], options?: HttpOptions) {
    return this.deleteItems(ids, options);
  }

  getFile(event: LazyLoadEvent, endpoint: string = 'csv'): Observable<any> {
    return this.getItemFile(event, endpoint);
  }
}
