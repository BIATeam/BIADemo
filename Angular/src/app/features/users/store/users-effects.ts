import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  create,
  failure,
  load,
  loadAllByPost,
  loadAllByPostSuccess,
  loadSuccess,
  remove,
  multiRemove,
  update,
  save,
  synchronize
} from './users-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { UserDas } from 'src/app/domains/user/services/user-das.service';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { User } from 'src/app/domains/user/model/user';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { getLastLazyLoadEvent } from './user.state';
import { UserFromADDas } from 'src/app/domains/user-from-AD/services/user-from-AD-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class UsersEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.userDas.getListByPost(event).pipe(
          map((result: DataResult<User[]>) => loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) => this.userDas.get(id).pipe(map((user) => loadSuccess({ user }))))
    )
  );

  synchronize$ = createEffect(() =>
    this.actions$.pipe(
      ofType(synchronize),
      concatMap((x) => of(x).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([x, event]) => {
        return this.userDas.synchronize().pipe(
          map(() => {
            this.biaMessageService.showSyncSuccess();
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(create),
      pluck('user'),
      concatMap((user) => of(user).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([user, event]) => {
        return this.userDas.post(user).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(update),
      pluck('user'),
      concatMap((user) => of(user).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([user, event]) => {
        return this.userDas.put(user, user.id).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.userDas.delete(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.userDas.deletes(ids).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(save),
      pluck('usersFromAD'),
      concatMap((usersFromAD) => of(usersFromAD).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([usersFromAD, event]) => {
        return this.userFromADDas.save(usersFromAD, '').pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            return loadAllByPost({ event: event });
          }),
          catchError((err) => {
            if (err.status === 303) {
              this.biaMessageService.showErrorDetail(err.error);
            } else {
              this.biaMessageService.showError();
            }
            return of(failure({ err: { concern: 'CREATE', error: err } }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private userDas: UserDas,
    private userFromADDas: UserFromADDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
