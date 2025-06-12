import {
  HTTP_INTERCEPTORS,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpStatusCode,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';
import { Observable, from, throwError } from 'rxjs';
import { catchError, filter, finalize, switchMap, take } from 'rxjs/operators';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import { HttpStatusCodeCustom } from 'src/app/shared/bia-shared/model/http-status-code-custom.enum';
import { allEnvironments } from 'src/environments/all-environments';
import { AuthService } from '../services/auth.service';
import { getCurrentCulture } from '../services/bia-translation.service';
import { RefreshTokenService } from '../services/refresh-token.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  protected isRefreshing = false;

  constructor(
    public authService: AuthService,
    // eslint-disable-next-line @typescript-eslint/no-deprecated
    public keycloakService: KeycloakService,
    protected appSettingsService: AppSettingsService
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.checkUrlNoToken(request.url)) {
      if (this.appSettingsService.appSettings?.keycloak?.isActive === true) {
        return this.launchRequestKeycloak(request, next);
      } else {
        return next.handle(this.addLanguageOnly(request));
      }
    }
    if (this.isRefreshing === false) {
      return this.launchRequest(request, next);
    } else {
      return this.waitLogin(request, next);
    }
  }

  protected checkUrlNoToken(url: string) {
    return (
      url.toLowerCase().indexOf(allEnvironments.urlAuth.toLowerCase()) > -1 ||
      url.toLowerCase().indexOf(allEnvironments.urlLog.toLowerCase()) > -1 ||
      url.toLowerCase().indexOf(allEnvironments.urlEnv.toLowerCase()) > -1 ||
      url.toLowerCase().indexOf('./assets/') > -1 ||
      (this.appSettingsService.appSettings?.keycloak?.isActive === true &&
        url
          .toLowerCase()
          .startsWith(
            this.appSettingsService.appSettings?.keycloak?.baseUrl
          ) === true)
    );
  }

  protected launchRequestKeycloak(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return from(this.keycloakService.getToken()).pipe(
      switchMap(jwtToken => {
        if (jwtToken?.length > 0) {
          request = this.addToken(request, jwtToken);
        }
        return next.handle(this.addLanguageOnly(request));
      })
    );
  }

  protected launchRequest(request: HttpRequest<any>, next: HttpHandler) {
    if (RefreshTokenService.shouldRefreshToken) {
      return this.handle401Error(request, next);
    }
    const jwtToken = this.authService.getToken();
    request = this.addToken(request, jwtToken);

    return next.handle(request).pipe(
      catchError(error => {
        if (
          error instanceof HttpErrorResponse &&
          (error.status === HttpStatusCode.Unauthorized ||
            error.status === HttpStatusCodeCustom.InvalidToken)
        ) {
          return this.handle401Error(request, next);
        } else {
          return throwError(() => error);
        }
      })
    );
  }

  protected addToken(request: HttpRequest<any>, token: string) {
    const langSelected = getCurrentCulture();
    return request.clone({
      withCredentials: false,
      setHeaders: {
        // eslint-disable-next-line @typescript-eslint/naming-convention
        Authorization: `Bearer ${token}`,
        // eslint-disable-next-line @typescript-eslint/naming-convention
        'Accept-Language': langSelected !== null ? langSelected : '',
      },
    });
  }

  protected addLanguageOnly(request: HttpRequest<any>) {
    const langSelected = getCurrentCulture();
    return request.clone({
      setHeaders: {
        // eslint-disable-next-line @typescript-eslint/naming-convention
        'Accept-Language': langSelected !== null ? langSelected : '',
      },
    });
  }

  protected handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    console.info('Handler 401');
    if (this.isRefreshing === false) {
      return this.login(request, next);
    } else {
      return this.waitLogin(request, next);
    }
  }

  protected login(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    this.isRefreshing = true;
    this.authService.logout();
    console.info('Login start from interceptor.');
    const obs$: Observable<HttpEvent<any>> = this.authService.login().pipe(
      switchMap((authInfo: AuthInfo) => {
        console.info('Login end from interceptor.');
        this.isRefreshing = false;
        return next.handle(this.addToken(request, authInfo.token));
      }),
      finalize(() => {
        // Requests can be canceled while login is ongoing.
        // If it happens, we must set the information that the refresh is over to
        // either let another request refresh the token
        // or inform that this request has correctly refreshed the token despite the cancelling
        if (this.isRefreshing) {
          this.isRefreshing = false;
          console.info('Login end from interceptor from finalize.');
        }
      })
    );

    if (this.appSettingsService.appSettings?.keycloak?.isActive === true) {
      return from([this.keycloakService.isLoggedIn()]).pipe(
        filter(x => x === true),
        switchMap(() => obs$)
      );
    } else {
      return obs$;
    }
  }

  protected waitLogin(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.authService.authInfo$.pipe(
      filter(authInfo => authInfo.token !== ''),
      take(1),
      switchMap(authInfo => {
        return next.handle(
          this.addToken(request, authInfo ? authInfo.token : '')
        );
      })
    );
  }
}

export const biaTokenInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: TokenInterceptor,
  multi: true,
};
