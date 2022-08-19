import { Injectable } from '@angular/core';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DomainAppSettingsActions } from './app-settings-actions';
import { AppSettingsDas } from '../services/app-settings-das.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { of } from 'rxjs';
import { AppSettings } from '../model/app-settings';
import { BiaOnlineOfflineService } from 'src/app/core/bia-core/services/bia-online-offline.service';
import { AppSettingsService } from '../services/app-settings.service';

const STORAGE_APPSETTINGS_KEY = 'AppSettings';

@Injectable()
export class AppSettingsEffects {
  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DomainAppSettingsActions.loadAll),
      switchMap(() =>
        this.appSettingsDas
          .get()
          .pipe(
            map((appSettings) => {
              if (BiaOnlineOfflineService.isModeEnabled === true) {
                localStorage.setItem(STORAGE_APPSETTINGS_KEY, JSON.stringify(appSettings));
              }
              this.appSettingsService.appSettings = appSettings;
              return DomainAppSettingsActions.loadAllSuccess({ appSettings });
            }),
            catchError((err) => {
              if (BiaOnlineOfflineService.isModeEnabled === true && BiaOnlineOfflineService.isServerAvailable(err) !== true) {
                const json: string | null = localStorage.getItem(STORAGE_APPSETTINGS_KEY);
                if (json) {
                  const appSettings = <AppSettings>JSON.parse(json);
                  this.appSettingsService.appSettings = appSettings;
                  return of(DomainAppSettingsActions.loadAllSuccess({ appSettings }));
                }
              }
              this.biaMessageService.showError();
              return of(DomainAppSettingsActions.failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private appSettingsDas: AppSettingsDas,
    private biaMessageService: BiaMessageService,
    private appSettingsService: AppSettingsService
  ) { }
}
