import {
  HTTP_INTERCEPTORS,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BiaOnlineOfflineService } from '../services/bia-online-offline.service';

@Injectable({
  providedIn: 'root',
})
export class BiaOnlineOfflineInterceptor implements HttpInterceptor {
  constructor(protected biaOnlineOfflineService: BiaOnlineOfflineService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError(error => {
        this.biaOnlineOfflineService.manageHttpErrorResponse(request, error);
        return throwError(() => error);
      })
    );
  }
}

export const biaOnlineOfflineInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: BiaOnlineOfflineInterceptor,
  multi: true,
};
