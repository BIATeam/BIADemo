import { Injectable, isDevMode, OnDestroy } from '@angular/core';
import { from, Observable, Subscription, throwError } from 'rxjs';
import { NotificationSignalRService } from 'src/app/domains/bia-domains/notification/services/notification-signalr.service';
import { DomainAppSettingsActions } from 'src/app/domains/bia-domains/app-settings/store/app-settings-actions';
import { AppState } from 'src/app/store/state';
import { Store } from '@ngrx/store';
import { allEnvironments } from 'src/environments/all-environments';
import { KeycloakService } from 'keycloak-angular';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';
import { catchError, filter, first, switchMap } from 'rxjs/operators';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { AppSettingsDas } from 'src/app/domains/bia-domains/app-settings/services/app-settings-das.service';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';

@Injectable({
  providedIn: 'root'
})
export class BiaAppInitService implements OnDestroy {
  protected sub: Subscription;
  constructor(
    protected authService: AuthService,
    protected appSettingsDas: AppSettingsDas,
    protected store: Store<AppState>,
    protected notificationSignalRService: NotificationSignalRService,
    protected keycloakService: KeycloakService) { }

  public initAuth() {
    return new Promise<void>((resolve) => {
      this.store.dispatch(DomainAppSettingsActions.loadAll());
      this.getObsAppSettings()
        .pipe(
          switchMap((appSettings: AppSettings | null) => {
            if (appSettings) {
              AppSettingsService.appSettings = appSettings;
            }

            if (AppSettingsService.appSettings?.keycloak?.isActive === true) {
              return from(this.initKeycloack(AppSettingsService.appSettings)).pipe(
                switchMap(() => this.getObsAuthInfo())
              );
            } else {
              return this.getObsAuthInfo();
            }
          })
        ).subscribe(() => {
          if (allEnvironments.enableNotifications === true) {
            this.notificationSignalRService.initialize();
          }

          resolve();
        });
    });
  }

  protected initKeycloack(appSettings: AppSettings): Promise<boolean> {
    return this.keycloakService.init({
      config: {
        url: appSettings.keycloak?.baseUrl,
        realm: appSettings.keycloak?.configuration.realm,
        clientId: appSettings.keycloak?.api.tokenConf.clientId,
      },
      enableBearerInterceptor: false,
      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri:
          window.location.origin + '/assets/silent-check-sso.html'
      }
    });
  }

  protected getObsAuthInfo(): Observable<AuthInfo> {
    return this.authService.login().pipe(first(), catchError((error) => this.catchError(error)));
  }

  protected getObsAppSettings(): Observable<AppSettings | null> {
    return this.store.select(getAppSettings)
      .pipe(
        filter(appSettings => !!appSettings && appSettings.environment?.type?.length > 0),
        first(),
        catchError((error) => this.catchError(error)));
  }

  protected catchError(error: any) {
    if (!isDevMode()) {
      window.location.href = environment.urlErrorPage + '?num=' + error.status;
    }
    return throwError(error);
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
