import {
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpRequest,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OnlineOfflineService } from '../services/online-offline.service';

@Injectable({
  providedIn: 'root'
})
export class OnlineOfflineInterceptor implements HttpInterceptor {

  constructor(protected onlineOfflineService: OnlineOfflineService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error) => {
        this.onlineOfflineService.manageHttpErrorResponse(request, error);
        return throwError(error);
      })
    );
  }
}

export const onlineOfflineInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: OnlineOfflineInterceptor,
  multi: true
};
