import { HttpClient, HttpErrorResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, interval, Observable, throwError } from 'rxjs';
import { catchError, first } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AppDB } from '../db';
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
export class OnlineOfflineService {

  protected serverAvailableSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public serverAvailable$: Observable<boolean> = this.serverAvailableSubject.asObservable();
  public static readonly httpHeaderRetry: string = 'X-HttpRequest-Retry';

  protected static _IsModeEnabled = false;
  public static get isModeEnabled() {
    return OnlineOfflineService._IsModeEnabled;
  }

  constructor(
    protected http: HttpClient,
    protected db: AppDB,
    protected biaMessageService: BiaMessageService,
    protected translateService: TranslateService) {
    OnlineOfflineService._IsModeEnabled = true;
    this.init();
  }

  public static isServerAvailable(error: any) {
    return error instanceof HttpErrorResponse && !(error.status === 0 || error.status === 504 || error.status === 503);
  }

  public static addHttpHeaderRetry(options?: HttpOptions) {
    if (options) {
      if (options.headers) {
        options.headers = (<HttpHeaders>options.headers).append(OnlineOfflineService.httpHeaderRetry, 'true');
      } else {
        options.headers = new HttpHeaders().append(OnlineOfflineService.httpHeaderRetry, 'true');
      }
    }
    else {
      options = {
        headers: new HttpHeaders().append(OnlineOfflineService.httpHeaderRetry, 'true')
      };
    }

    return options;
  }

  public manageHttpErrorResponse(httpRequest: HttpRequest<any>, httpErrorResponse: HttpErrorResponse) {
    if (OnlineOfflineService.isServerAvailable(httpErrorResponse) !== true) {
      if (httpRequest.headers?.has(OnlineOfflineService.httpHeaderRetry) === true) {
        this.addHttpRequestItem(httpRequest);
        this.biaMessageService.showWarning(this.translateService.instant('biaMsg.serverUnavailableDataSaveLocally'));
      }
      this.serverAvailableSubject.next(false);
    }
  }

  protected init() {
    this.checkServerAvailable();
  }

  /**
   * Checks at regular intervals if the backend is available.
   */
  protected checkServerAvailable() {
    interval(2000).subscribe(() => {
      if (this.serverAvailableSubject.value === false) {
        this.ping().pipe(
          first()
        ).subscribe((ping) => {
          if (ping?.length > 0) {
            this.sendHttpRequestsFromIndexedDb();
            this.serverAvailableSubject.next(true);
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

      this.biaMessageService.showSyncSuccess();
    }
  }

  protected httpRequestPost(httpRequestItem: HttpRequestItem) {
    this.http.post(httpRequestItem.httpRequest.urlWithParams, httpRequestItem.httpRequest.body).pipe(
      first(),
      catchError((error) => {
        if (OnlineOfflineService.isServerAvailable(error) === true) {
          this.deleteHttpRequestItem(httpRequestItem);
        }
        return throwError(error);
      })
    ).subscribe(
      () => {
        this.deleteHttpRequestItem(httpRequestItem);
      },
    );
  }

  protected httpRequestPut(httpRequestItem: HttpRequestItem) {
    this.http.put(httpRequestItem.httpRequest.urlWithParams, httpRequestItem.httpRequest.body).pipe(
      first(),
      catchError((error) => {
        if (OnlineOfflineService.isServerAvailable(error) === true) {
          this.deleteHttpRequestItem(httpRequestItem);
        }
        return throwError(error);
      })
    ).subscribe(
      () => {
        this.deleteHttpRequestItem(httpRequestItem);
      },
    );
  }

  protected httpRequestDelete(httpRequestItem: HttpRequestItem) {
    this.http.delete(httpRequestItem.httpRequest.urlWithParams).pipe(
      first(),
      catchError((error) => {
        if (OnlineOfflineService.isServerAvailable(error) === true) {
          this.deleteHttpRequestItem(httpRequestItem);
        }
        return throwError(error);
      })
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
