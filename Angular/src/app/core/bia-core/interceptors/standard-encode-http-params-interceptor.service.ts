import {
  HttpInterceptor,
  HttpRequest,
  HttpEvent,
  HttpHandler,
  HttpParams,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { StandardEncoder } from './standard-encoder';

// Workaroud issue: https://github.com/angular/angular/issues/11058
// Because dot.NET interpret '+' as space
export class StandardEncodeHttpParamsInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const params = new HttpParams({ encoder: new StandardEncoder(), fromString: req.params.toString() });
    return next.handle(req.clone({ params }));
  }
}

export const standardEncodeHttpParamsInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: StandardEncodeHttpParamsInterceptor,
  multi: true
};
