import {
  HTTP_INTERCEPTORS,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppSettingsService } from '../app-settings/services/app-settings.service';
import { BiaAppConstantsService } from '../services/bia-app-constants.service';
// import { environment } from 'src/environments/environment';

@Injectable()
export class XhrWithCredInterceptorService implements HttpInterceptor {
  constructor(protected appSettingsService: AppSettingsService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (
      !BiaAppConstantsService.environment.useXhrWithCred ||
      this.appSettingsService.appSettings?.keycloak?.isActive === true
    ) {
      return next.handle(req);
    }
    const authReq = req.clone({
      withCredentials: true,
    });
    return next.handle(authReq);
  }
}

export const biaXhrWithCredInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: XhrWithCredInterceptorService,
  multi: true,
};
