import { HttpClient, HttpErrorResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { liveQuery } from 'dexie';
import { BehaviorSubject, from, Observable, Subscription, timer } from 'rxjs';
import { filter, first, map, skip } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AppDB } from '../db';
import { AuthService } from './auth.service';
import { BiaMessageService } from './bia-message.service';
import { HttpOptions } from './generic-das.service';

export interface HttpRequestItem {
  id?: number;
  httpRequest: HttpRequest<any>
}

enum HTTPMethod {
  DELETE = 'DELETE',
  GET = 'GET',
  OPTIONS = 'OPTIONS',
  POST = 'POST',
  PUT = 'PUT',
}

@Injectable({
  providedIn: 'root'
})
export class BiaOnlineOfflineService implements OnDestroy {

  protected serverAvailableSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public serverAvailable$: Observable<boolean> = this.serverAvailableSubject.asObservable();
  public syncCompleted$: Observable<boolean>;
  public static readonly httpHeaderRetry: string = 'X-HttpRequest-Retry';
  protected sub = new Subscription();
  protected static _IsModeEnabled = false;
  public static get isModeEnabled() {
    return BiaOnlineOfflineService._IsModeEnabled;
  }

  constructor(
    protected http: HttpClient,
    protected db: AppDB,
    protected biaMessageService: BiaMessageService,
    protected translateService: TranslateService,
    protected authService: AuthService) {
    BiaOnlineOfflineService._IsModeEnabled = true;
    this.init();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  public static isServerAvailable(error: any) {
    return error instanceof HttpErrorResponse && !(error.status === 0 || error.status === 504 || error.status === 503);
  }

  public static addHttpHeaderRetry(options?: HttpOptions) {
    if (options) {
      if (options.headers) {
        options.headers = (<HttpHeaders>options.headers).append(BiaOnlineOfflineService.httpHeaderRetry, 'true');
      } else {
        options.headers = new HttpHeaders().append(BiaOnlineOfflineService.httpHeaderRetry, 'true');
      }
    }
    else {
      options = {
        headers: new HttpHeaders().append(BiaOnlineOfflineService.httpHeaderRetry, 'true')
      };
    }

    return options;
  }

  public manageHttpErrorResponse(httpRequest: HttpRequest<any>, httpErrorResponse: HttpErrorResponse) {
    if (BiaOnlineOfflineService.isServerAvailable(httpErrorResponse) !== true) {
      if (httpRequest.headers?.has(BiaOnlineOfflineService.httpHeaderRetry) === true) {
        this.addHttpRequestItem(httpRequest);
        this.biaMessageService.showWarning(this.translateService.instant('biaMsg.serverUnavailableDataSaveLocally'));
      }
      this.serverAvailableSubject.next(false);
    }
  }

  protected init() {
    this.checkServerAvailable();
    this.initObsSyncCompleted();
  }

  protected initObsSyncCompleted() {
    this.syncCompleted$ = from(liveQuery(() => this.db.httpRequests.count())).pipe(map((x: Number) => x < 1));

    this.sub.add(
      this.syncCompleted$.pipe(skip(1), filter(x => x === true)).subscribe(() =>
        this.biaMessageService.showSyncSuccess()
      )
    );
  }

  /**
   * Checks at regular intervals if the backend is available.
   */
  protected checkServerAvailable() {
    timer(0, 2000).subscribe(() => {
      if (this.serverAvailableSubject.value === false) {
        this.ping().pipe(
          first()
        ).subscribe((ping) => {
          if (ping?.length > 0 && this.serverAvailableSubject.value === false) {
            this.serverAvailableSubject.next(true);
            this.authService.shouldRefreshToken = true;
            this.sendHttpRequestsFromIndexedDb();
          }
        });
      }
    });
  }

  protected ping(): Observable<string> {
    return this.http.get<string>(environment.logging.conf.serverLoggingUrl + '/ping');
  }

  /**
   * Retrieve HttpRequests stored in local database and execute them.
   */
  protected async sendHttpRequestsFromIndexedDb() {
    const httpRequestItems: HttpRequestItem[] = await this.db.httpRequests.toArray();
    if (httpRequestItems?.length > 0) {
      httpRequestItems.forEach((httpRequestItem: HttpRequestItem) => {
        if (httpRequestItem.httpRequest.method == HTTPMethod.POST) {
          this.httpRequestPost(httpRequestItem);
        } else if (httpRequestItem.httpRequest.method == HTTPMethod.PUT) {
          this.httpRequestPut(httpRequestItem);

        } else if (httpRequestItem.httpRequest.method == HTTPMethod.DELETE) {
          this.httpRequestDelete(httpRequestItem);
        }
      });
    }
  }

  protected httpRequestPost(httpRequestItem: HttpRequestItem) {
    this.http.post(httpRequestItem.httpRequest.urlWithParams, httpRequestItem.httpRequest.body).pipe(
      first()
    ).subscribe(
      () => {
        this.deleteHttpRequestItem(httpRequestItem);
      },
    );
  }

  protected httpRequestPut(httpRequestItem: HttpRequestItem) {
    this.http.put(httpRequestItem.httpRequest.urlWithParams, httpRequestItem.httpRequest.body).pipe(
      first()
    ).subscribe(
      () => {
        this.deleteHttpRequestItem(httpRequestItem);
      },
    );
  }

  protected httpRequestDelete(httpRequestItem: HttpRequestItem) {
    this.http.delete(httpRequestItem.httpRequest.urlWithParams).pipe(
      first()
    ).subscribe(
      () => {
        this.deleteHttpRequestItem(httpRequestItem);
      },
    );
  }

  protected addHttpRequestItem(httpRequest: HttpRequest<any>) {
    this.db.httpRequests.add(<HttpRequestItem>{ httpRequest: { ...httpRequest } });
  }

  protected deleteHttpRequestItem(httpRequestItem: HttpRequestItem) {
    if (httpRequestItem.id) {
      this.db.httpRequests.delete(httpRequestItem.id);
    }
  }
}
