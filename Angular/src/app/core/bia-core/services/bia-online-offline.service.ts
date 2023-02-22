import { HttpClient, HttpErrorResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable, Subscription, throwError, timer } from 'rxjs';
import { catchError, filter, first, skip } from 'rxjs/operators';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { AppDB } from '../db';
import { AuthService } from './auth.service';
import { BiaEnvironmentService } from './bia-environment.service';
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
  protected syncCompletedSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public syncCompleted$: Observable<boolean> = this.syncCompletedSubject.asObservable();
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
    protected authService: AuthService,
    protected appSettingsService: AppSettingsService) {
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
    this.checkSyncCompleted();
    this.initObsSyncCompleted();
    this.checkServerAvailable();
  }

  protected initObsSyncCompleted() {
    this.sub.add(this.syncCompleted$.pipe(skip(1), filter(x => x === true)).subscribe(() => this.biaMessageService.showSyncSuccess()));
  }

  protected async checkSyncCompleted() {
    const nbHttpRequestItems: number = await this.db.httpRequests.count();
    const syncCompleted: boolean = nbHttpRequestItems < 1;
    if (syncCompleted !== this.syncCompletedSubject.value) {
      this.syncCompletedSubject.next(syncCompleted);
    }
  }

  /**
   * Checks at regular intervals if the backend is available.
   */
  protected checkServerAvailable() {
    timer(500, 2000).subscribe(() => {
      if (this.serverAvailableSubject.value === false && this.appSettingsService.appSettings) {
        this.ping().pipe(
          first()
        ).subscribe((ping) => {
          if (ping?.length > 0 && this.serverAvailableSubject.value === false) {
            this.serverAvailableSubject.next(true);
            this.authService.shouldRefreshToken = true;
            this.launchFirstHttpRequestItem();
          }
        });
      }
    });
  }

  protected ping(): Observable<string> {
    return this.http.get<string>(BiaEnvironmentService.getServerLoggingUrl() + '/ping');
  }

  /**
   * Retrieve HttpRequests stored in local database and execute them.
   */
  protected async sendHttpRequest(httpRequestItem: HttpRequestItem | null) {
    if (httpRequestItem) {
      if (httpRequestItem.httpRequest.method == HTTPMethod.POST) {
        this.httpRequestPost(httpRequestItem);
      } else if (httpRequestItem.httpRequest.method == HTTPMethod.PUT) {
        this.httpRequestPut(httpRequestItem);
      } else if (httpRequestItem.httpRequest.method == HTTPMethod.DELETE) {
        this.httpRequestDelete(httpRequestItem);
      }
    }
  }

  protected httpRequestPost(httpRequestItem: HttpRequestItem) {
    const obs$ = this.http.post(httpRequestItem.httpRequest.urlWithParams, httpRequestItem.httpRequest.body);
    return this.httpRequest(httpRequestItem, obs$);
  }

  protected httpRequestPut(httpRequestItem: HttpRequestItem) {
    const obs$ = this.http.put(httpRequestItem.httpRequest.urlWithParams, httpRequestItem.httpRequest.body);
    return this.httpRequest(httpRequestItem, obs$);
  }

  protected httpRequestDelete(httpRequestItem: HttpRequestItem) {
    const obs$ = this.http.delete(httpRequestItem.httpRequest.urlWithParams);
    return this.httpRequest(httpRequestItem, obs$);
  }

  protected httpRequest(httpRequestItem: HttpRequestItem, obs$: Observable<any>) {
    return obs$.pipe(
      first(),
      catchError((error) => {
        if (BiaOnlineOfflineService.isServerAvailable(error) === true && error.status !== 498) {
          this.deleteHttpRequestItem(httpRequestItem);
        } else {
          this.launchNextHttpRequestItem(httpRequestItem);
        }
        return throwError(error);
      })
    ).subscribe(
      () => {
        this.deleteHttpRequestItem(httpRequestItem);
      },
    );
  }

  protected async addHttpRequestItem(httpRequest: HttpRequest<any>) {
    await this.db.httpRequests.add(<HttpRequestItem>{ httpRequest: { ...httpRequest } });
    await this.checkSyncCompleted();
  }

  protected async deleteHttpRequestItem(httpRequestItem: HttpRequestItem) {
    if (httpRequestItem && httpRequestItem.id) {
      const nextHttpRequestItem: HttpRequestItem | null = await this.takeNextHttpRequest(httpRequestItem);
      await this.db.httpRequests.delete(httpRequestItem.id);
      await this.checkSyncCompleted();
      if (nextHttpRequestItem) {
        await this.sendHttpRequest(nextHttpRequestItem);
      }
    }
  }

  protected async launchFirstHttpRequestItem() {
    const nextHttpRequestItem: HttpRequestItem | null = await this.takeFirstHttpRequest();
    if (nextHttpRequestItem) {
      await this.sendHttpRequest(nextHttpRequestItem);
    }
  }

  protected async launchNextHttpRequestItem(httpRequestItem: HttpRequestItem) {
    if (httpRequestItem && httpRequestItem.id) {
      const nextHttpRequestItem: HttpRequestItem | null = await this.takeNextHttpRequest(httpRequestItem);
      if (nextHttpRequestItem) {
        await this.sendHttpRequest(nextHttpRequestItem);
      }
    }
  }

  protected async takeFirstHttpRequest() {
    const httpRequestItems: HttpRequestItem[] = await this.getAllHttpRequestItem();
    if (httpRequestItems?.length > 0) {
      return httpRequestItems[0];
    }

    return null;
  }

  protected async takeNextHttpRequest(httpRequestItem: HttpRequestItem) {
    const httpRequestItems: HttpRequestItem[] = await this.getAllHttpRequestItem();

    if (httpRequestItems?.length > 0) {
      const index = httpRequestItems.findIndex(x => x.id === httpRequestItem.id);
      return httpRequestItems[index + 1];
    }

    return null;
  }

  protected async getAllHttpRequestItem() {
    return await this.db.httpRequests.orderBy('id').toArray();
  }
}
