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
  private readonly keycloakService = inject(Keycloak);
  private readonly keycloakEventSignal = inject(KEYCLOAK_EVENT_SIGNAL);
  private keycloakReady$: BehaviorSubject<boolean> =
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
          switchMap((appSettings: AppSettings | null) => {
            return this.keycloakReady$.pipe(
              filter(x => x === true),
              switchMap(() => {
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

  private setupKeycloakEvents(): void {
    effect(() => {
      const keycloakEvent = this.keycloakEventSignal();

      if (
        keycloakEvent.type === KeycloakEventType.AuthLogout ||
        keycloakEvent.type === KeycloakEventType.Ready ||
        keycloakEvent.type === KeycloakEventType.TokenExpired
      ) {
        if (this.keycloakService.authenticated !== true) {
          this.keycloakService.login({
            redirectUri: window.location.href,
            idpHint:
              this.appSettingsService.appSettings?.keycloak?.configuration
                ?.idpHint,
          });
        } else {
          this.keycloakReady$.next(true);
        }
      }
    });
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
    // Keycloak events using Angular effects are automatically cleaned up

    if (BiaAppConstantsService.allEnvironments.enableNotifications === true) {
      this.notificationSignalRService.destroy();
    }
  }
}
