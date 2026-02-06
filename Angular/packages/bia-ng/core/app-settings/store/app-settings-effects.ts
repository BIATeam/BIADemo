import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AppSettings } from 'packages/bia-ng/models/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { BiaAppConstantsService } from '../../services/bia-app-constants.service';
import { BiaMessageService } from '../../services/bia-message.service';
import { BiaOnlineOfflineService } from '../../services/bia-online-offline.service';
import { DynamicPermissionService } from '../../services/dynamic-permission.service';
import { AppSettingsDas } from '../services/app-settings-das.service';
import { AppSettingsService } from '../services/app-settings.service';
import { CoreAppSettingsActions } from './app-settings-actions';

@Injectable()
export class AppSettingsEffects {
  protected actions$: Actions = inject(Actions);
  protected appSettingsDas: AppSettingsDas = inject(AppSettingsDas);
  protected biaMessageService: BiaMessageService = inject(BiaMessageService);
  protected appSettingsService: AppSettingsService = inject(AppSettingsService);
  protected dynamicPermissionService: DynamicPermissionService = inject(
    DynamicPermissionService
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CoreAppSettingsActions.loadAll),
      switchMap(() =>
        this.appSettingsDas.get().pipe(
          map(appSettings => {
            if (BiaOnlineOfflineService.isModeEnabled === true) {
              localStorage.setItem(
                BiaAppConstantsService.storageAppSettingsKey(),
                JSON.stringify(appSettings)
              );
            }
            this.appSettingsService.appSettings = appSettings;

            // Initialize Permission registry from backend
            if (appSettings?.permissions) {
              this.dynamicPermissionService.initialize(appSettings.permissions);
            }

            return CoreAppSettingsActions.loadAllSuccess({ appSettings });
          }),
          catchError(err => {
            if (
              BiaOnlineOfflineService.isModeEnabled === true &&
              BiaOnlineOfflineService.isServerAvailable(err) !== true
            ) {
              const json: string | null = localStorage.getItem(
                BiaAppConstantsService.storageAppSettingsKey()
              );
              if (json) {
                const appSettings = <AppSettings>JSON.parse(json);
                this.appSettingsService.appSettings = appSettings;

                // Initialize Permission registry from cached AppSettings (offline mode)
                if (appSettings?.permissions) {
                  this.dynamicPermissionService.initialize(
                    appSettings.permissions
                  );
                }

                return of(
                  CoreAppSettingsActions.loadAllSuccess({ appSettings })
                );
              }
            }
            this.biaMessageService.showErrorHttpResponse(err);
            return of(CoreAppSettingsActions.failure({ error: err }));
          })
        )
      )
    )
  );
}
