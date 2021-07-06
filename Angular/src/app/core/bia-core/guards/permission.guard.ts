import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivate {
  constructor(private authService: AuthService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    const permission = route.data.permission as string;
    return this.authService.hasPermissionObs(permission).pipe(
      tap((hasPermission: boolean) => {
        if (hasPermission !== true) {
          window.location.href = environment.urlErrorPage + '?num=403';
        }
      })
    );
  }
}
