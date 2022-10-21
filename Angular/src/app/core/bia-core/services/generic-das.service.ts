import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { catchError, first, map, tap } from 'rxjs/operators';
import { from, NEVER, Observable, of, throwError } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { DateHelperService } from './date-helper.service';
import { MatomoTracker } from './matomo/matomo-tracker.service';
import { BiaOnlineOfflineService } from './bia-online-offline.service';
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
  endpoint?: string;
}

export interface GetParam extends HttpParam {
  id?: string | number;
}

export interface GetListParam extends HttpParam {
}

export interface GetListByPostParam extends HttpParam {
  event: LazyLoadEvent;
}

export interface SaveParam<TIn> extends HttpParam {
  items: TIn[],
}

export interface PutParam<TIn> extends HttpParam {
  item: TIn;
  id: string | number;
}

export interface PostParam<TIn> extends HttpParam {
  item: TIn;
}

export interface DeleteParam extends HttpParam {
  id: string | number;
}

export interface DeletesParam extends HttpParam {
  ids: string[] | number[];
}

export abstract class GenericDas {
  public http: HttpClient;
  public route: string;
  protected matomoTracker: MatomoTracker;
  protected db: AppDB;

  constructor(protected injector: Injector, protected endpoint: string) {
    this.http = injector.get<HttpClient>(HttpClient);
    this.route = GenericDas.buildRoute(endpoint);
    this.matomoTracker = injector.get<MatomoTracker>(MatomoTracker);
    if (BiaOnlineOfflineService.isModeEnabled === true) {
      this.db = injector.get<AppDB>(AppDB);
    }
  }

  public static buildRoute(endpoint: string): string {
    let route = '/' + endpoint + '/';
    route = route.replace('//', '/');
    return environment.apiUrl + route;
  }

  concatRoute(route: string, endpoint: string | undefined)
  {
    return route + `${endpoint ? endpoint + '/' : ''}`.replace('//', '/');
  }

  getItem<TOut>(param?: GetParam): Observable<TOut> {
    const url = `${this.concatRoute(this.route,param?.endpoint)}${param?.id ?? ''}`;
    //const url = `${this.route}${param?.endpoint ?? ''}${param?.id ?? ''}`;

    let obs$ = this.http.get<TOut>(url, param?.options).pipe(
      map((data) => {
        DateHelperService.fillDate(data);
        this.translateItem(data);
        return data;
      })
    );

    if (param?.offlineMode === true && BiaOnlineOfflineService.isModeEnabled === true) {
      obs$ = this.getWithCatchErrorOffline(obs$, url);
    }

    return obs$;
  }

  getListItems<TOut>(param?: GetListParam): Observable<TOut[]> {
    const url = `${this.route}${param?.endpoint ?? ''}`;

    let obs$ = this.http.get<TOut[]>(url, param?.options).pipe(
      map((items) => {
        items.forEach((item) => {
          DateHelperService.fillDate(item);
          this.translateItem(item);
        });
        return items;
      }));

    if (param?.offlineMode === true && BiaOnlineOfflineService.isModeEnabled === true) {
      obs$ = this.getWithCatchErrorOffline(obs$, url);
    }

    return obs$;
  }

  getListItemsByPost<TOut>(param: GetListByPostParam): Observable<DataResult<TOut[]>> {
    if (!param.event) {
      return of();
    }

    param.endpoint = param.endpoint ?? 'all';

    return this.http.post<TOut[]>(`${this.route}${param.endpoint}`, param.event, { observe: 'response' }).pipe(
      map((resp: HttpResponse<TOut[]>) => {
        const totalCount = Number(resp.headers.get('X-Total-Count'));
        const datas = resp.body ? resp.body : [];
        datas.forEach((data) => {
          DateHelperService.fillDate(data);
          this.translateItem(data);
        });

        const dataResult = {
          totalCount,
          data: datas
        } as DataResult<TOut[]>;
        return dataResult;
      })
    );
  }

  saveItem<TIn, TOut>(param: SaveParam<TIn>) {
    param.endpoint = param.endpoint ?? 'save';
    if (param.items) {
      param.items.forEach((item) => {
        DateHelperService.fillDate(item);
      });
    }

    const url = `${this.route}${param.endpoint}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(this.http.post<TOut>(url, param.items, param.options));
    } else {
      return this.http.post<TOut>(url, param.items, param.options);
    }
  }

  putItem<TIn, TOut>(param: PutParam<TIn>) {
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDate(param.item);

    const url = `${this.route}${param.endpoint}${param.id}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(this.http.put<TOut>(url, param.item, param.options));
    } else {
      return this.http.put<TOut>(url, param.item, param.options);
    }
  }

  postItem<TIn, TOut>(param: PostParam<TIn>) {
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDate(param.item);

    const url = `${this.route}${param.endpoint}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(this.http.post<TOut>(url, param.item, param.options));
    } else {
      return this.http.post<TOut>(url, param.item, param.options);
    }
  }

  deleteItem(param: DeleteParam) {
    param.endpoint = param.endpoint ?? '';

    const url = `${this.route}${param.endpoint}${param.id}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(this.http.delete<void>(url, param.options));
    } else {
      return this.http.delete<void>(url, param.options);
    }
  }

  deleteItems(param: DeletesParam) {
    param.endpoint = param.endpoint ?? '';

    const url = `${this.route}${param.endpoint}?ids=${param.ids.join('&ids=')}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(this.http.delete<void>(url, param.options));
    } else {
      return this.http.delete<void>(url, param.options);
    }
  }

  getItemFile(event: LazyLoadEvent, endpoint: string = 'csv'): Observable<any> {
    this.matomoTracker.trackDownload('Export ' + endpoint);
    return this.http.post(`${this.route}${endpoint}`, event, {
      responseType: 'blob',
      headers: new HttpHeaders().append('Content-Type', 'application/json')
    });
  }

  translateItem<TOut>(item: TOut) {
    return item;
  }

  protected setWithCatchErrorOffline(obs$: Observable<any>) {
    return obs$.pipe(
      catchError((error) => {
        if (BiaOnlineOfflineService.isModeEnabled === true && BiaOnlineOfflineService.isServerAvailable(error) !== true) {
          return NEVER;
        }
        return throwError(error);
      })
    );
  }

  protected getWithCatchErrorOffline(obs$: Observable<any>, url: string) {
    return obs$.pipe(
      tap((result: any) => {
        this.clearDataByUrl(url);
        this.addDataTtem(url, result);
      }),
      catchError((error) => {
        if (BiaOnlineOfflineService.isModeEnabled === true && BiaOnlineOfflineService.isServerAvailable(error) !== true) {
          return from(this.db.datas.get(url)).pipe(
            first(),
            map((dataItem: DataItem | undefined) => dataItem ? dataItem.data : undefined)
          );
        }
        return throwError(error);
      })
    );
  }

  protected clearDataByUrl(url: string) {
    this.db.datas.delete(url);
  }

  protected addDataTtem(url: string, result: any) {
    const data: DataItem = <DataItem>{ url: url, data: result };
    this.db.datas?.add(data);
  }
}
