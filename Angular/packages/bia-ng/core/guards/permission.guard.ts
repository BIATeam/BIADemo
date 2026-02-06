import { APP_BASE_HREF } from '@angular/common';
import { HttpStatusCode } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Permission } from 'src/app/shared/permission';
import { BiaPermission } from '../bia-permission';
//import { allEnvironments } from 'src/environments/all-environments';
import { AuthService } from '../services/auth.service';
import { BiaAppConstantsService } from '../services/bia-app-constants.service';

@Injectable({
  providedIn: 'root',
})
export class PermissionGuard {
  constructor(
    protected authService: AuthService,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    const permission = route.data.permission as
      | string
      | Permission
      | BiaPermission;
    return this.authService.hasPermissionObs(permission).pipe(
      tap((hasPermission: boolean) => {
        if (hasPermission !== true) {
          if (window.location.href === this.baseHref) {
            window.location.href =
              BiaAppConstantsService.allEnvironments.urlErrorPage +
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
