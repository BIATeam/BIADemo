import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { DateHelperService } from './date-helper.service';

export interface HttpOptions {
  headers?:
    | HttpHeaders
    | {
        [header: string]: string | string[];
      };
  observe?: any;
  params?:
    | HttpParams
    | {
        [param: string]: string | string[];
      };
  reportProgress?: boolean;
  responseType?: any;
  withCredentials?: boolean;
}

export abstract class GenericDas {
  public http: HttpClient;
  public route: string;

  constructor(injector: Injector, endpoint: string) {
    this.http = injector.get<HttpClient>(HttpClient);
    this.route = GenericDas.buildRoute(endpoint);
  }

  public static buildRoute(endpoint: string): string {
    let route = '/' + endpoint + '/';
    route = route.replace('//', '/');
    return environment.apiUrl + route;
  }

  getItem<TOut>(id: string | number, options?: HttpOptions): Observable<TOut> {
    return this.http.get<TOut>(`${this.route}${id}`, options).pipe(
      map((data) => {
        DateHelperService.fillDate(data);
        return data;
      })
    );
  }

  getListItems<TOut>(endpoint: string = '', options?: HttpOptions): Observable<TOut[]> {
    return this.http.get<TOut[]>(`${this.route}${endpoint}`, options).pipe(
      map((datas) => {
        datas.forEach((data) => {
          DateHelperService.fillDate(data);
        });
        return datas;
      })
    );
  }

  getListItemsByPost<TOut>(event: LazyLoadEvent, endpoint: string = 'all'): Observable<DataResult<TOut[]>> {
    if (!event) {
      return of();
    }
    return this.http.post<TOut[]>(`${this.route}${endpoint}`, event, { observe: 'response' }).pipe(
      map((resp: HttpResponse<TOut[]>) => {
        const totalCount = Number(resp.headers.get('X-Total-Count'));
        const datas = resp.body ? resp.body : [];
        datas.forEach((data) => {
          DateHelperService.fillDate(data);
        });

        const dataResult = {
          totalCount,
          data: datas
        } as DataResult<TOut[]>;
        return dataResult;
      })
    );
  }

  saveItem<TIn, TOut>(items: TIn[], endpoint: string = 'save', options?: HttpOptions) {
    if (items) {
      items.forEach((item) => {
        DateHelperService.fillDate(item);
      });
    }
    return this.http.post<TOut>(`${this.route}${endpoint}`, items, options);
  }

  putItem<TIn, TOut>(item: TIn, id: string | number, options?: HttpOptions) {
    DateHelperService.fillDate(item);
    return this.http.put<TOut>(`${this.route}${id}`, item, options);
  }

  postItem<TIn, TOut>(item: TIn, options?: HttpOptions) {
    DateHelperService.fillDate(item);
    return this.http.post<TOut>(this.route, item, options);
  }

  deleteItem(id: string | number, options?: HttpOptions) {
    return this.http.delete<void>(`${this.route}${id}`, options);
  }

  deleteItems(ids: string[] | number[], options?: HttpOptions) {
    return this.http.delete<void>(`${this.route}?ids=${ids.join('&ids=')}`, options);
  }

  getItemFile(event: LazyLoadEvent, endpoint: string = 'csv'): Observable<any> {
    return this.http.post(`${this.route}${endpoint}`, event, {
      responseType: 'blob',
      headers: new HttpHeaders().append('Content-Type', 'application/json')
    });
  }
}
