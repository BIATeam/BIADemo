import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { catchError, first, map } from 'rxjs/operators';
import { from, NEVER, Observable, of, throwError } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { DateHelperService } from './date-helper.service';
import { MatomoTracker } from './matomo/matomo-tracker.service';
import { OnlineOfflineService } from './online-offline.service';
import { AppDB, DataItem } from '../db';

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

interface HttpParam {
  offlineMode?: boolean;
  options?: HttpOptions;
}

// interface GetParam extends HttpParam {
//   id?: string | number; 
//   endpoint: '';
// }

// interface GetListParam extends HttpParam{
//   endpoint: '';
// }

// interface GetListByPostParam extends HttpParam {
// event: LazyLoadEvent;
// endpoint: 'all';
// }

// interface SaveParam<TIn> extends HttpParam {
//   items: TIn[],
//   endpoint: 'save';
// }

export interface PutParam<TIn> extends HttpParam {
  item: TIn;
  id: string | number;
  endpoint?: '';
}

export interface PostParam<TIn> extends HttpParam {
  item: TIn;
  endpoint?: '';
}

// interface DeleteParam<TIn> extends HttpParam {
//   id: string | number;
//   endpoint: '';
// }

// interface DeletesParam<TIn> extends HttpParam {
//   ids: string[] | number[];
//   endpoint: '';
// }

export abstract class GenericDas {
  public http: HttpClient;
  public route: string;
  protected matomoTracker: MatomoTracker;
  protected db: AppDB;

  constructor(protected injector: Injector, protected endpoint: string) {
    this.http = injector.get<HttpClient>(HttpClient);
    this.route = GenericDas.buildRoute(endpoint);
    this.matomoTracker = injector.get<MatomoTracker>(MatomoTracker);
    if (OnlineOfflineService.isModeEnabled === true) {
      this.db = injector.get<AppDB>(AppDB);
    }
  }

  public static buildRoute(endpoint: string): string {
    let route = '/' + endpoint + '/';
    route = route.replace('//', '/');
    return environment.apiUrl + route;
  }

  getItem<TOut>(id?: string | number, options?: HttpOptions): Observable<TOut> {
    return this.http.get<TOut>(id ? `${this.route}${id}` : `${this.route}`, options).pipe(
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

  getListItems2<TOut>(endpoint: string = '', options?: HttpOptions): Observable<TOut[]> {
    const url = `${this.route}${endpoint}`;
    const obs$ = this.http.get<TOut[]>(url, options).pipe(
      map((datas) => {
        datas.forEach((data) => {
          DateHelperService.fillDate(data);
        });
        return datas;
      }),
      catchError((error) => {
        if (OnlineOfflineService.isModeEnabled === true && OnlineOfflineService.isServerAvailable(error) !== true) {
          return from(this.db.datas.filter(function (x) {
            return x.url === url;
          }).toArray()).pipe(
            map((dataItems: DataItem[]) => dataItems.map((dataItem) => dataItem.data))
          );
        }
        return throwError(error);
      })
    );

    if (OnlineOfflineService.isModeEnabled === true) {
      obs$.pipe(first()).subscribe((results: TOut[]) => {
        this.db.datas.filter(function (x) {
          return x.url === url;
        }).delete();
        results.forEach((res) => {
          const data: DataItem = <DataItem>{ url: url, data: res };
          this.db.datas?.add(data);
        });
      });
    }

    return obs$;
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

  putItem<TIn, TOut>(param: PutParam<TIn>) {
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDate(param.item);

    if (param.offlineMode === true) {
      param.options = OnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.execWithCatchErrorOffline(this.http.put<TOut>(`${this.route}${param.id}`, param.item, param.options));
    } else {
      return this.http.put<TOut>(`${this.route}${param.id}`, param.item, param.options);
    }
  }

  postItem<TIn, TOut>(param: PostParam<TIn>) {
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDate(param.item);

    if (param.offlineMode === true) {
      param.options = OnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.execWithCatchErrorOffline(this.http.post<TOut>(this.route, param.item, param.options));
    } else {
      return this.http.post<TOut>(this.route, param.item, param.options);
    }
  }

  deleteItem(id: string | number, options?: HttpOptions) {
    return this.http.delete<void>(`${this.route}${id}`, options);
  }

  deleteItemWithRetry(id: string | number, options?: HttpOptions) {
    options = OnlineOfflineService.addHttpHeaderRetry(options);
    return this.execWithCatchErrorOffline(this.deleteItem(id, options));
  }

  deleteItems(ids: string[] | number[], options?: HttpOptions) {
    return this.http.delete<void>(`${this.route}?ids=${ids.join('&ids=')}`, options);
  }

  deleteItemsWithRetry(ids: string[] | number[], options?: HttpOptions) {
    options = OnlineOfflineService.addHttpHeaderRetry(options);
    return this.execWithCatchErrorOffline(this.deleteItems(ids, options));
  }

  getItemFile(event: LazyLoadEvent, endpoint: string = 'csv'): Observable<any> {
    this.matomoTracker.trackDownload('Export ' + endpoint);
    return this.http.post(`${this.route}${endpoint}`, event, {
      responseType: 'blob',
      headers: new HttpHeaders().append('Content-Type', 'application/json')
    });
  }

  protected execWithCatchErrorOffline(verbMethod: Observable<any>) {
    return verbMethod.pipe(
      catchError((error) => {
        if (OnlineOfflineService.isModeEnabled === true && OnlineOfflineService.isServerAvailable(error) !== true) {
          return NEVER;
        }
        return throwError(error);
      })
    );
  }
}
