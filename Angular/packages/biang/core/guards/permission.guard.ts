import { APP_BASE_HREF } from '@angular/common';
import { HttpStatusCode } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
//import { allEnvironments } from 'src/environments/all-environments';
import { BiaEnvironmentService } from '../public-api';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard {
  constructor(
    protected authService: AuthService,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    const permission = route.data.permission as string;
    return this.authService.hasPermissionObs(permission).pipe(
      tap((hasPermission: boolean) => {
        if (hasPermission !== true) {
          if (window.location.href === this.baseHref) {
            window.location.href =
              BiaEnvironmentService.allEnvironments.urlErrorPage +
              '?num=' +
              HttpStatusCode.Forbidden;
          } else {
            location.assign(this.baseHref);
          }
        }
      })
    );
  }
}
