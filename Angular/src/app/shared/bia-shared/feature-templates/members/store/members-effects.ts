import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import {
  catchError,
  concatMap,
  map,
  switchMap,
  withLatestFrom,
} from 'rxjs/operators';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { memberCRUDConfiguration } from '../member.constants';
import { Member } from '../model/member';
import { MemberDas } from '../services/member-das.service';
import { MemberService } from '../services/member.service';
import { FeatureMembersStore } from './member.state';
import { FeatureMembersActions } from './members-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class MembersEffects {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.loadAllByPost),
      map(x => x?.event),
      switchMap(event =>
        this.memberDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<Member[]>) =>
            FeatureMembersActions.loadAllByPostSuccess({
              result: result,
              event: event,
            })
          ),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureMembersActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.load),
      map(x => x?.id),
      switchMap(id => {
        return this.memberDas.get({ id: id }).pipe(
          map(member => FeatureMembersActions.loadSuccess({ member })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(FeatureMembersActions.failure({ error: err }));
          })
        );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.create),
      map(x => x?.member),
      concatMap(member =>
        of(member).pipe(
          withLatestFrom(
            this.store.select(FeatureMembersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([member, event]) => {
        return this.memberDas
          .post({
            item: member,
            offlineMode: memberCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              this.memberService.parentService?.refreshList();
              if (memberCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMembersActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureMembersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  createMulti$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.createMulti),
      map(x => x?.members),
      concatMap(member =>
        of(member).pipe(
          withLatestFrom(
            this.store.select(FeatureMembersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([members, event]) => {
        return this.memberDas
          .postItem({
            item: members,
            endpoint: 'addMulti',
            offlineMode: memberCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              this.memberService.parentService?.refreshList();
              if (memberCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMembersActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureMembersActions.failure({ error: err }));
            })
          );
      })
    )
  );
  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.update),
      map(x => x?.member),
      concatMap(member =>
        of(member).pipe(
          withLatestFrom(
            this.store.select(FeatureMembersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([member, event]) => {
        return this.memberDas
          .put({
            item: member,
            id: member.id,
            offlineMode: memberCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              this.memberService.parentService?.refreshList();
              if (memberCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMembersActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureMembersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  save$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.save),
      map(x => x?.members),
      concatMap(members =>
        of(members).pipe(
          withLatestFrom(
            this.store.select(FeatureMembersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([members, event]) => {
        return this.memberDas
          .save({
            items: members,
            offlineMode: memberCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (memberCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMembersActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureMembersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  destroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.remove),
      map(x => x?.id),
      concatMap((id: number) =>
        of(id).pipe(
          withLatestFrom(
            this.store.select(FeatureMembersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([id, event]) => {
        return this.memberDas
          .delete({
            id: id,
            offlineMode: memberCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              this.memberService.parentService?.refreshList();
              if (memberCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMembersActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureMembersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  multiDestroy$ = createEffect(() =>
    this.actions$.pipe(
      ofType(FeatureMembersActions.multiRemove),
      map(x => x?.ids),
      concatMap((ids: number[]) =>
        of(ids).pipe(
          withLatestFrom(
            this.store.select(FeatureMembersStore.getLastLazyLoadEvent)
          )
        )
      ),
      switchMap(([ids, event]) => {
        return this.memberDas
          .deletes({
            ids: ids,
            offlineMode: memberCRUDConfiguration.useOfflineMode,
          })
          .pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              this.memberService.parentService?.refreshList();
              if (memberCRUDConfiguration.useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return FeatureMembersActions.loadAllByPost({
                  event: event,
                });
              }
            }),
            catchError(err => {
              this.biaMessageService.showErrorHttpResponse(err);
              return of(FeatureMembersActions.failure({ error: err }));
            })
          );
      })
    )
  );

  constructor(
    protected actions$: Actions,
    protected memberDas: MemberDas,
    protected memberService: MemberService,
    protected biaMessageService: BiaMessageService,
    protected store: Store<AppState>
  ) {}
}
