import { Injectable } from '@angular/core';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadDomainAppSettings,
  loadDomainAppSettingsSuccess
} from './app-settings-actions';
import { AppSettingsDas } from '../services/app-settings-das.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { of } from 'rxjs';

@Injectable()
export class AppSettingsEffects {
  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadDomainAppSettings),
      switchMap(() =>
        this.appSettingsDas
          .get()
          .pipe(
            map((appSettings) => loadDomainAppSettingsSuccess({ appSettings })),
            catchError((err) => {
              this.biaMessageService.showError();
              return of(failure({ error: err }));
            })
          )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private appSettingsDas: AppSettingsDas,
    private biaMessageService: BiaMessageService
  ) {}
}
