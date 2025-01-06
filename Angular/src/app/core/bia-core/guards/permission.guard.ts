import { APP_BASE_HREF } from '@angular/common';
import { HttpStatusCode } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { allEnvironments } from 'src/environments/all-environments';

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
              allEnvironments.urlErrorPage + '?num=' + HttpStatusCode.Forbidden;
          } else {
            location.assign(this.baseHref);
          }
        }
      })
    );
  }
}
