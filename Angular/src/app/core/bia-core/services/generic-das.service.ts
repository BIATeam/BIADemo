import { APP_BASE_HREF } from '@angular/common';
import {
  HttpClient,
  HttpHeaders,
  HttpResponse,
  HttpStatusCode,
} from '@angular/common/http';
import { Injector } from '@angular/core';
import { TableLazyLoadEvent } from 'primeng/table';
import { NEVER, Observable, from, of, throwError } from 'rxjs';
import { catchError, first, map, tap } from 'rxjs/operators';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { clone } from 'src/app/shared/bia-shared/utils';
import { AppDB, DataItem } from '../db';
import {
  DeleteParam,
  DeletesParam,
  GetListByPostParam,
  GetListParam,
  GetParam,
  PostParam,
  PutParam,
  SaveParam,
} from '../models/http-params';
import { BiaEnvironmentService } from './bia-environment.service';
import { BiaOnlineOfflineService } from './bia-online-offline.service';
import { DateHelperService } from './date-helper.service';
import { MatomoTracker } from './matomo/matomo-tracker.service';

export abstract class GenericDas {
  public http: HttpClient;
  public route: string;
  protected matomoTracker: MatomoTracker;
  protected db: AppDB;
  protected baseHref: string;

  constructor(
    protected injector: Injector,
    protected endpoint: string
  ) {
    this.baseHref = this.injector.get(APP_BASE_HREF);
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
    return BiaEnvironmentService.getApiUrl() + route;
  }

  concatRoute(route: string, endpoint: string | undefined) {
    return route + `${endpoint ? endpoint + '/' : ''}`.replace('//', '/');
  }

  getItem<TOut>(param?: GetParam): Observable<TOut> {
    const url = `${this.concatRoute(this.route, param?.endpoint)}${
      param?.id ?? ''
    }`;
    //const url = `${this.route}${param?.endpoint ?? ''}${param?.id ?? ''}`;

    let obs$ = this.http.get<TOut>(url, param?.options).pipe(
      map(data => {
        DateHelperService.fillDate(data);
        this.translateItem(data);
        return data;
      }),
      catchError(error => {
        // Example: if I am on an element and I change of Team,
        // and this Team does not access to this current element,
        // we return to the root of the site.
        if (
          error.status === HttpStatusCode.Unauthorized ||
          error.status === HttpStatusCode.Forbidden ||
          error.status == HttpStatusCode.NotFound
        ) {
          location.assign(this.baseHref);
        }
        return throwError(() => error);
      })
    );

    if (
      param?.offlineMode === true &&
      BiaOnlineOfflineService.isModeEnabled === true
    ) {
      obs$ = this.getWithCatchErrorOffline(obs$, url);
    }

    return obs$;
  }

  getListItems<TOut>(param?: GetListParam): Observable<TOut[]> {
    const url = `${this.route}${param?.endpoint ?? ''}`;

    let obs$ = this.http.get<TOut[]>(url, param?.options).pipe(
      map(items => {
        items.forEach(item => {
          DateHelperService.fillDate(item);
          this.translateItem(item);
        });
        return items;
      })
    );

    if (
      param?.offlineMode === true &&
      BiaOnlineOfflineService.isModeEnabled === true
    ) {
      obs$ = this.getWithCatchErrorOffline(obs$, url);
    }

    return obs$;
  }

  getListItemsByPost<TOut>(
    param: GetListByPostParam
  ): Observable<DataResult<TOut[]>> {
    if (!param.event) {
      return of();
    }

    param.endpoint = param.endpoint ?? 'all';

    return this.http
      .post<TOut[]>(`${this.route}${param.endpoint}`, param.event, {
        observe: 'response',
      })
      .pipe(
        map((resp: HttpResponse<TOut[]>) => {
          const totalCount = Number(resp.headers.get('X-Total-Count'));
          const datas = resp.body ? resp.body : [];
          datas.forEach(data => {
            DateHelperService.fillDate(data);
            this.translateItem(data);
          });

          const dataResult = {
            totalCount,
            data: datas,
          } as DataResult<TOut[]>;
          return dataResult;
        })
      );
  }

  saveItem<TIn, TOut>(param: SaveParam<TIn>) {
    // param might contains ngrx state item which is immutable : clone to allow update
    param = clone(param);
    param.endpoint = param.endpoint ?? 'save';
    if (param.items) {
      param.items.forEach(item => {
        DateHelperService.fillDate(item);
      });
    }

    const url = `${this.route}${param.endpoint}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(
        this.http.post<TOut>(url, param.items, param.options)
      );
    } else {
      return this.http.post<TOut>(url, param.items, param.options);
    }
  }

  putItem<TIn, TOut>(param: PutParam<TIn>) {
    // param might contains ngrx state item which is immutable : clone to allow update
    param = clone(param);
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDate(param.item);

    const url = `${this.route}${param.endpoint}${param.id}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(
        this.http.put<TOut>(url, param.item, param.options)
      );
    } else {
      return this.http.put<TOut>(url, param.item, param.options).pipe(
        map(data => {
          DateHelperService.fillDate(data);
          this.translateItem(data);
          return data;
        })
      );
    }
  }

  postItem<TIn, TOut>(param: PostParam<TIn>) {
    // param might contains ngrx state item which is immutable : clone to allow update
    param = clone(param);
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDate(param.item);

    const url = `${this.route}${param.endpoint}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(
        this.http.post<TOut>(url, param.item, param.options)
      );
    } else {
      return this.http.post<TOut>(url, param.item, param.options).pipe(
        map(data => {
          DateHelperService.fillDate(data);
          this.translateItem(data);
          return data;
        })
      );
    }
  }

  deleteItem(param: DeleteParam) {
    param.endpoint = param.endpoint ?? '';

    const url = `${this.route}${param.endpoint}${param.id}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(
        this.http.delete<void>(url, param.options)
      );
    } else {
      return this.http.delete<void>(url, param.options);
    }
  }

  deleteItems(param: DeletesParam) {
    param.endpoint = param.endpoint ?? '';

    const url = `${this.route}${param.endpoint}?ids=${param.ids.join('&ids=')}`;
    if (param.offlineMode === true) {
      param.options = BiaOnlineOfflineService.addHttpHeaderRetry(param.options);
      return this.setWithCatchErrorOffline(
        this.http.delete<void>(url, param.options)
      );
    } else {
      return this.http.delete<void>(url, param.options);
    }
  }

  getItemFile(event: TableLazyLoadEvent, endpoint = 'csv'): Observable<any> {
    this.matomoTracker.trackDownload('Export ' + endpoint);
    return this.http.post(`${this.route}${endpoint}`, event, {
      responseType: 'blob',
      headers: new HttpHeaders().append('Content-Type', 'application/json'),
    });
  }

  translateItem<TOut>(item: TOut) {
    return item;
  }

  protected setWithCatchErrorOffline(obs$: Observable<any>) {
    return obs$.pipe(
      catchError(error => {
        if (
          BiaOnlineOfflineService.isModeEnabled === true &&
          BiaOnlineOfflineService.isServerAvailable(error) !== true
        ) {
          return NEVER;
        }
        return throwError(() => error);
      })
    );
  }

  protected getWithCatchErrorOffline(obs$: Observable<any>, url: string) {
    return obs$.pipe(
      tap((result: any) => {
        this.clearDataByUrl(url);
        this.addDataTtem(url, result);
      }),
      catchError(error => {
        if (
          BiaOnlineOfflineService.isModeEnabled === true &&
          BiaOnlineOfflineService.isServerAvailable(error) !== true
        ) {
          return from(this.db.datas.get(url)).pipe(
            first(),
            map((dataItem: DataItem | undefined) =>
              dataItem ? dataItem.data : undefined
            )
          );
        }
        return throwError(() => error);
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
