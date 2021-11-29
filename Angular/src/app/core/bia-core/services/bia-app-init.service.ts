import { Injectable, OnDestroy } from '@angular/core';
import { AuthService } from './auth.service';
import { Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { isDevMode } from '@angular/core';
import { NotificationSignalRService } from 'src/app/domains/notification/services/notification-signalr.service';
import { loadDomainAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings-actions';
import { AppState } from 'src/app/store/state';
import { Store } from '@ngrx/store';

@Injectable({
  providedIn: 'root'
})
export class BiaAppInitService implements OnDestroy {
  private sub: Subscription;
  constructor(
    private authService: AuthService,
    private store: Store<AppState>,
    private notificationSignalRService: NotificationSignalRService) { }

  Init() {
    return this.initAuth();
  }

  private initAuth() {
    return new Promise<void>((resolve) => {
      this.sub = this.authService
        .login()
        .pipe(
          catchError((error) => {
            if (!isDevMode()) {
              window.location.href = environment.urlErrorPage + '?num=' + error.status;
            }
            return throwError(error);
          })
        )
        .subscribe(() => {
          // Load app settings:
          this.store.dispatch(loadDomainAppSettings());

          if (environment.enableNotifications === true) {
            this.notificationSignalRService.initialize();
          }

          resolve();
        });
    });
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }

    if (environment.enableNotifications === true) {
      this.notificationSignalRService.destroy();
    }
  }
}
