import {
  effect,
  inject,
  Injectable,
  isDevMode,
  OnDestroy,
} from '@angular/core';
import { Store } from '@ngrx/store';
import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType } from 'keycloak-angular';
import Keycloak from 'keycloak-js';
import { AppSettings, AuthInfo } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, first, switchMap } from 'rxjs/operators';
import { AppSettingsDas } from '../app-settings/services/app-settings-das.service';
import { AppSettingsService } from '../app-settings/services/app-settings.service';
import { CoreAppSettingsActions } from '../app-settings/store/app-settings-actions';
import { getAppSettings } from '../app-settings/store/app-settings.state';
import { NotificationSignalRService } from '../notification/services/notification-signalr.service';
import { AuthService } from './auth.service';
import { BiaAppConstantsService } from './bia-app-constants.service';

@Injectable({
  providedIn: 'root',
})
export class BiaAppInitService implements OnDestroy {
  protected readonly keycloakService = inject(Keycloak);
  protected readonly keycloakEventSignal = inject(KEYCLOAK_EVENT_SIGNAL);
  protected readonly keycloakReady$: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);

  constructor(
    protected authService: AuthService,
    protected appSettingsDas: AppSettingsDas,
    protected store: Store<BiaAppState>,
    protected notificationSignalRService: NotificationSignalRService,
    protected appSettingsService: AppSettingsService
  ) {
    this.setupKeycloakEvents();
  }

  public initAuth() {
    return new Promise<void>(resolve => {
      this.store.dispatch(CoreAppSettingsActions.loadAll());
      this.getObsAppSettings()
        .pipe(
          filter((appSettings: AppSettings | null) => appSettings !== null),
          switchMap(() => {
            return this.keycloakReady$.pipe(
              filter(x => x === true),
              switchMap(() => {
                this.loginOnKeycloak();
                return this.getObsAuthInfo();
              })
            );
          })
        )
        .subscribe(() => {
          if (
            BiaAppConstantsService.allEnvironments.enableNotifications === true
          ) {
            this.notificationSignalRService.initialize();
          }

          resolve();
        });
    });
  }

  protected setupKeycloakEvents(): void {
    effect(() => {
      const keycloakEvent = this.keycloakEventSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.keycloakReady$.next(true);
      }

      if (
        keycloakEvent.type === KeycloakEventType.AuthLogout ||
        keycloakEvent.type === KeycloakEventType.TokenExpired
      ) {
        this.loginOnKeycloak();
      }
    });
  }

  protected loginOnKeycloak(): void {
    if (this.keycloakService.authenticated !== true) {
      this.keycloakService.login({
        redirectUri: window.location.href,
        idpHint:
          this.appSettingsService.appSettings?.keycloak?.configuration?.idpHint,
      });
    }
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
        BiaAppConstantsService.allEnvironments.urlErrorPage +
        '?num=' +
        error.status;
    }
    return throwError(() => error);
  }

  ngOnDestroy() {
    if (BiaAppConstantsService.allEnvironments.enableNotifications === true) {
      this.notificationSignalRService.destroy();
    }
  }
}
