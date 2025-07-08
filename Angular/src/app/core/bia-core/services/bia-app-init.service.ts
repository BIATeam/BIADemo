import { Injectable, isDevMode, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  KeycloakEventLegacy,
  KeycloakEventTypeLegacy,
  KeycloakService,
} from 'keycloak-angular';
import { Observable, Subscription, throwError } from 'rxjs';
import { catchError, filter, first, switchMap } from 'rxjs/operators';
import { AppSettings } from 'src/app/domains/bia-domains/app-settings/model/app-settings';
import { AppSettingsDas } from 'src/app/domains/bia-domains/app-settings/services/app-settings-das.service';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { DomainAppSettingsActions } from 'src/app/domains/bia-domains/app-settings/store/app-settings-actions';
import { getAppSettings } from 'src/app/domains/bia-domains/app-settings/store/app-settings.state';
import { NotificationSignalRService } from 'src/app/domains/bia-domains/notification/services/notification-signalr.service';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';
import { AppState } from 'src/app/store/state';
import { allEnvironments } from 'src/environments/all-environments';
import { AuthService } from './auth.service';

// const STORAGE_KEYCLOAK_TOKEN = 'KeyCloak_Token';
// const STORAGE_KEYCLOAK_REFRESHTOKEN = 'KeyCloak_RefreshToken';
// const STORAGE_KEYCLOAK_IDTOKEN = 'KeyCloak_IdToken';

@Injectable({
  providedIn: 'root',
})
export class BiaAppInitService implements OnDestroy {
  protected sub = new Subscription();
  constructor(
    protected authService: AuthService,
    protected appSettingsDas: AppSettingsDas,
    protected store: Store<AppState>,
    protected notificationSignalRService: NotificationSignalRService,
    // eslint-disable-next-line @typescript-eslint/no-deprecated
    protected keycloakService: KeycloakService,
    protected appSettingsService: AppSettingsService
  ) {}

  public initAuth() {
    return new Promise<void>(resolve => {
      this.store.dispatch(DomainAppSettingsActions.loadAll());
      this.getObsAppSettings()
        .pipe(
          switchMap((appSettings: AppSettings | null) => {
            if (appSettings?.keycloak?.isActive === true) {
              return this.initKeycloack(appSettings);
            } else {
              return this.getObsAuthInfo();
            }
          })
        )
        .subscribe(() => {
          if (allEnvironments.enableNotifications === true) {
            this.notificationSignalRService.initialize();
          }

          resolve();
        });
    });
  }

  protected initKeycloack(appSettings: AppSettings): Observable<AuthInfo> {
    this.initEventKeycloakLogin();
    const obs$: Observable<AuthInfo> = this.initEventKeycloakSuccess();

    // const token = localStorage.getItem(STORAGE_KEYCLOAK_TOKEN);
    // const refreshToken = localStorage.getItem(STORAGE_KEYCLOAK_REFRESHTOKEN);
    // const idToken = localStorage.getItem(STORAGE_KEYCLOAK_IDTOKEN);

    this.keycloakService.init({
      config: {
        url: appSettings.keycloak?.baseUrl,
        realm: appSettings.keycloak?.configuration.realm,
        clientId: appSettings.keycloak?.api.tokenConf.clientId,
      },
      enableBearerInterceptor: false,
      initOptions: {
        onLoad: 'check-sso',
        // checkLoginIframe: false,
        enableLogging: isDevMode(),
        silentCheckSsoRedirectUri:
          window.location.origin + '/assets/bia/html/silent-check-sso.html',
        // token: token ?? undefined,
        // refreshToken: refreshToken ?? undefined,
        // idToken: idToken ?? undefined,
      },
    });

    return obs$;
  }

  protected initEventKeycloakSuccess(): Observable<AuthInfo> {
    return this.keycloakService.keycloakEvents$.asObservable().pipe(
      filter(
        // eslint-disable-next-line @typescript-eslint/no-deprecated
        (keycloakEvent: KeycloakEventLegacy) =>
          // eslint-disable-next-line @typescript-eslint/no-deprecated
          keycloakEvent?.type === KeycloakEventTypeLegacy.OnAuthSuccess ||
          // eslint-disable-next-line @typescript-eslint/no-deprecated
          keycloakEvent?.type === KeycloakEventTypeLegacy.OnAuthRefreshSuccess
      ),
      first(),
      switchMap(() => {
        return this.getObsAuthInfo();
      })
    );
  }

  protected initEventKeycloakLogin(): void {
    this.sub.add(
      this.keycloakService.keycloakEvents$
        .asObservable()
        // eslint-disable-next-line @typescript-eslint/no-deprecated
        .subscribe(async (keycloakEvent: KeycloakEventLegacy) => {
          if (
            // eslint-disable-next-line @typescript-eslint/no-deprecated
            keycloakEvent?.type === KeycloakEventTypeLegacy.OnAuthLogout ||
            // eslint-disable-next-line @typescript-eslint/no-deprecated
            keycloakEvent?.type === KeycloakEventTypeLegacy.OnReady ||
            // eslint-disable-next-line @typescript-eslint/no-deprecated
            keycloakEvent?.type === KeycloakEventTypeLegacy.OnTokenExpired
          ) {
            if (this.keycloakService.isLoggedIn() !== true) {
              this.keycloakService.login({
                redirectUri: window.location.href,
                idpHint:
                  this.appSettingsService.appSettings?.keycloak?.configuration
                    ?.idpHint,
                // scope: 'offline_access',
              });
            }
          } /*else if (keycloakEvent?.type == KeycloakEventType.OnAuthSuccess) {

        const token = await this.keycloakService.getToken();
        localStorage.setItem(STORAGE_KEYCLOAK_TOKEN, token);

        const refreshToken = await this.keycloakService.getKeycloakInstance().refreshToken;
        localStorage.setItem(STORAGE_KEYCLOAK_REFRESHTOKEN, refreshToken ?? '');

        const idToken = await this.keycloakService.getKeycloakInstance().idToken;
        localStorage.setItem(STORAGE_KEYCLOAK_IDTOKEN, idToken ?? '');
      }*/
        })
    );
  }

  protected getObsAuthInfo(): Observable<AuthInfo> {
    console.info('Login from app init.');
    return this.authService.login().pipe(
      first(),
      catchError(error => this.catchError(error))
    );
  }

  protected getObsAppSettings(): Observable<AppSettings | null> {
    return this.store.select(getAppSettings).pipe(
      filter(
        appSettings =>
          !!appSettings && appSettings.environment?.type?.length > 0
      ),
      first(),
      catchError(error => this.catchError(error))
    );
  }

  protected catchError(error: any) {
    if (!isDevMode()) {
      window.location.href =
        allEnvironments.urlErrorPage + '?num=' + error.status;
    }
    return throwError(() => error);
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
