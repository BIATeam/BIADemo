import { Injectable } from '@angular/core';
import { map, startWith, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  loadDomainEnvironmentConfiguration,
  loadDomainEnvironmentConfigurationSuccess
} from './environment-configuration-actions';
import { EnvironmentConfigurationDas } from '../services/environment-configuration-das.service';

@Injectable()
export class EnvironmentConfigurationsEffects {
  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadDomainEnvironmentConfiguration),
      startWith(loadDomainEnvironmentConfiguration()),
      switchMap(() =>
        this.environmentConfigurationDas
          .get()
          .pipe(map((environmentConfiguration) => loadDomainEnvironmentConfigurationSuccess({ environmentConfiguration })))
      )
    )
  );

  constructor(private actions$: Actions, private environmentConfigurationDas: EnvironmentConfigurationDas) {}
}
