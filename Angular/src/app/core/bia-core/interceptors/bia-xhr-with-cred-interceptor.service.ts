import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class XhrWithCredInterceptorService implements HttpInterceptor {
  constructor(protected appSettingsService: AppSettingsService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!environment.useXhrWithCred || this.appSettingsService.appSettings?.keycloak?.isActive === true) {
      return next.handle(req);
    }
    const authReq = req.clone({
      withCredentials: true
    });
    return next.handle(authReq);
  }
}


export const biaXhrWithCredInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: XhrWithCredInterceptorService,
  multi: true
};
