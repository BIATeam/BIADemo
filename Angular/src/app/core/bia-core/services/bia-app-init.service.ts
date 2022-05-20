import { Injectable, isDevMode, OnDestroy } from '@angular/core';
import { Subscription, throwError } from 'rxjs';
import { NotificationSignalRService } from 'src/app/domains/bia-domains/notification/services/notification-signalr.service';
import { DomainAppSettingsActions } from 'src/app/domains/bia-domains/app-settings/store/app-settings-actions';
import { AppState } from 'src/app/store/state';
import { Store } from '@ngrx/store';
import { allEnvironments } from 'src/environments/all-environments';
import { KeycloakService } from 'keycloak-angular';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BiaAppInitService implements OnDestroy {
  protected sub: Subscription;
  constructor(
    protected authService: AuthService,
    protected store: Store<AppState>,
    protected notificationSignalRService: NotificationSignalRService,
    protected keycloakService: KeycloakService) { }

  public initAuth() {
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
          this.store.dispatch(DomainAppSettingsActions.loadAll());

          if (allEnvironments.enableNotifications === true) {
            this.notificationSignalRService.initialize();
          }

          resolve();
        });
    });
  }

  public init(): void {
    this.store.dispatch(DomainAppSettingsActions.loadAll());

    if (allEnvironments.enableNotifications === true) {
      this.notificationSignalRService.initialize();
    }
  }

  public initKeycloack(): Promise<boolean> {
    return this.keycloakService.init({
      config: {
        url: environment.keycloak.conf.authServerUrl,
        realm: environment.keycloak.conf.realm,
        clientId: environment.keycloak.conf.resource
      },
      enableBearerInterceptor: false,
      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri:
          window.location.origin + '/assets/silent-check-sso.html'
      }
    })
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }

    if (allEnvironments.enableNotifications === true) {
      this.notificationSignalRService.destroy();
    }
  }
}
