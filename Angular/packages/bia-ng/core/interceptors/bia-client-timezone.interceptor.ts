import {
  HTTP_INTERCEPTORS,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class BiaClientTimeZoneInterceptor implements HttpInterceptor {
  private readonly timeZone: string;

  constructor() {
    this.timeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.headers.has('X-Client-TimeZone')) {
      return next.handle(req);
    }

    const cloned = req.clone({
      setHeaders: {
        'X-Client-TimeZone': this.timeZone,
      },
    });

    return next.handle(cloned);
  }
}

export const biaClientTimeZoneInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: BiaClientTimeZoneInterceptor,
  multi: true,
};
