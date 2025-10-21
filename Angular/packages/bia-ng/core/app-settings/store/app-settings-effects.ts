import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AppSettings } from 'packages/bia-ng/models/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { BiaAppConstantsService } from '../../services/bia-app-constants.service';
import { BiaMessageService } from '../../services/bia-message.service';
import { BiaOnlineOfflineService } from '../../services/bia-online-offline.service';
import { AppSettingsDas } from '../services/app-settings-das.service';
import { AppSettingsService } from '../services/app-settings.service';
import { CoreAppSettingsActions } from './app-settings-actions';

@Injectable()
export class AppSettingsEffects {
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

  constructor(
    protected actions$: Actions,
    protected appSettingsDas: AppSettingsDas,
    protected biaMessageService: BiaMessageService,
    protected appSettingsService: AppSettingsService
  ) {}
}
