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
  save
} from './members-actions';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './member.state';
import { Member } from '../model/member';
import { MemberDas } from '../services/member-das.service';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class MembersEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.memberDas.getListByPost(event).pipe(
          map((result: DataResult<Member[]>) => loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) => {
        return this.memberDas.get(id).pipe(
          map((member) => loadSuccess({ member })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(create),
      pluck('member'),
      concatMap((member) => of(member).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([member, event]) => {
        return this.memberDas.post(member).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            return loadAllByPost({ event: <LazyLoadEvent>event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(update),
      pluck('member'),
      concatMap((member) => of(member).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([member, event]) => {
        return this.memberDas.put(member, member.id).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllByPost({ event: <LazyLoadEvent>event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
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
        return this.memberDas.delete(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllByPost({ event: <LazyLoadEvent>event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
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
        return this.memberDas.deletes(ids).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            // Uncomment this if you do not use SignalR to refresh
            return loadAllByPost({ event: <LazyLoadEvent>event });
            // Uncomment this if you use SignalR to refresh
            // return biaSuccessWaitRefreshSignalR();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(save),
      pluck('members'),
      concatMap((members) => of(members).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([members, event]) => {
        return this.memberDas.save(members).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            return loadAllByPost({ event: <LazyLoadEvent>event });
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private memberDas: MemberDas,
    private biaMessageService: BiaMessageService,
    private store: Store<AppState>
  ) {}
}
