import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { FeatureMembersActions } from './members-actions';
import { MemberDas } from '../services/member-das.service';
import { Store } from '@ngrx/store';
import { getLastLazyLoadEvent } from './member.state';
import { Member } from '../model/member';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class MembersEffects {
  static useSignalR = false;
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.memberDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Member[]>) => FeatureMembersActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMembersActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.load),
      pluck('id'),
      switchMap((id) => {
        return this.memberDas.get({ id: id }).pipe(
          map((member) => FeatureMembersActions.loadSuccess({ member })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMembersActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.create),
      pluck('member'),
      concatMap((member) => of(member).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([member, event]) => {
        return this.memberDas.post({ item: member }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (MembersEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMembersActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMembersActions.failure({ error: err }));
          })
        );
      })
    )
  );

  createMulti$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.createMulti),
      pluck('members'),
      concatMap((member) => of(member).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([members, event]) => {
        return this.memberDas.postItem({ item: members, endpoint:"addMulti" }).pipe(
          map(() => {
            this.biaMessageService.showAddSuccess();
            if (MembersEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMembersActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMembersActions.failure({ error: err }));
          })
        );
      })
    )
  );


  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.update),
      pluck('member'),
      concatMap((member) => of(member).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([member, event]) => {
        return this.memberDas.put({ item: member, id: member.id }).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            if (MembersEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMembersActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMembersActions.failure({ error: err }));
          })
        );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.remove),
      pluck('id'),
      concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([id, event]) => {
        return this.memberDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (MembersEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMembersActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMembersActions.failure({ error: err }));
          })
        );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.multiRemove),
      pluck('ids'),
      concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(getLastLazyLoadEvent)))),
      switchMap(([ids, event]) => {
        return this.memberDas.deletes({ ids: ids }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            if (MembersEffects.useSignalR) {
              return biaSuccessWaitRefreshSignalR();
            } else {
              return FeatureMembersActions.loadAllByPost({ event: <LazyLoadEvent>event });
            }
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(FeatureMembersActions.failure({ error: err }));
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
