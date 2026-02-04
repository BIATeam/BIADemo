import { HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import {
  AbstractDas,
  BiaOnlineOfflineService,
  clone,
  DateHelperService,
} from '@bia-team/bia-ng/core';
import {
  Announcement,
  DataResult,
  GetListByPostParam,
  GetListParam,
  GetParam,
  PostParam,
  PutParam,
  SaveParam,
} from '@bia-team/bia-ng/models';
import { catchError, map, Observable, of, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementDas extends AbstractDas<Announcement> {
  constructor(injector: Injector) {
    super(injector, 'Announcements');
  }

  getItem<TOut>(param?: GetParam): Observable<TOut> {
    const url = `${this.concatRoute(this.route, param?.endpoint)}${
      param?.id ?? ''
    }`;
    let obs$ = this.http.get<TOut>(url, param?.options).pipe(
      map(data => {
        DateHelperService.fillDate(data);
        this.translateItem(data);
        return data;
      }),
      catchError(error => {
        if (
          param?.baseHrefRedirectionOnError !== false &&
          (error.status === HttpStatusCode.Unauthorized ||
            error.status === HttpStatusCode.Forbidden ||
            error.status === HttpStatusCode.NotFound)
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
    param = clone(param);
    param.endpoint = param.endpoint ?? 'save';
    if (param.items) {
      param.items.forEach(item => {
        DateHelperService.fillDateISO(item);
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
    param = clone(param);
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDateISO(param.item);

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
    param = clone(param);
    param.endpoint = param.endpoint ?? '';
    DateHelperService.fillDateISO(param.item);

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
}
