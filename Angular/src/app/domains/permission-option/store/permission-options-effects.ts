import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadAllPermissionOptions,
  loadAllPermissionOptionsSuccess,
} from './permission-options-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { PermissionOptionDas } from '../services/permission-option-das.service';
/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class PermissionOptionsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllPermissionOptions) /* When action is dispatched */,
      /* startWith(loadAll()), */
      /* Hit the Permissions Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Permissions Reducers' will take care of the rest */
      switchMap(() =>
        this.permissionDas.getList('allOptions').pipe(
          map((permissions) => loadAllPermissionOptionsSuccess({ permissions })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  /*
  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) =>
        this.permissionDas.get(id).pipe(
          map((permission) => loadSuccess({ permission })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );
  */

  constructor(
    private actions$: Actions,
    private permissionDas: PermissionOptionDas,
    private biaMessageService: BiaMessageService
  ) {}
}
