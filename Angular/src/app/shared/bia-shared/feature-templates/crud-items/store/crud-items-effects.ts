import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, withLatestFrom, concatMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { CrudItemsActions } from './crud-items-actions';
import { CrudItemDas } from '../services/crud-item-das.service';
import { Store } from '@ngrx/store';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';
import { AppState } from 'src/app/store/state';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { LazyLoadEvent } from 'primeng/api';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { useSignalR } from '../crud-item.constants';
import { BaseDto } from '../../../model/base-dto';
import { CrudItemState } from './crud-item.state';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class CrudItemsEffects<CrudItem extends BaseDto> {
  loadAllByPost$ = createEffect(() =>
    this.actions$.pipe(
      ofType(this.crudItemsActions.loadAllByPost),
      pluck('event'),
      switchMap((event) =>
        this.crudItemDas.getListByPost({ event: event }).pipe(
          map((result: DataResult<CrudItem[]>) => this.crudItemsActions.loadAllByPostSuccess({ result: result, event: event })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(this.crudItemsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(this.crudItemsActions.load),
      pluck('id'),
      switchMap((id) => {
        return this.crudItemDas.get({ id: id }).pipe(
          map((crudItem) => this.crudItemsActions.loadSuccess({ crudItem })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(this.crudItemsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    protected actions$: Actions,
    protected crudItemDas: CrudItemDas<CrudItem>,
    protected biaMessageService: BiaMessageService,
    protected store: Store<AppState>,
    protected crudItemState: CrudItemState<CrudItem>,
    protected crudItemsActions: CrudItemsActions<CrudItem>,
  ) {

    
    // create
    createEffect(() =>
      this.actions$.pipe(
        ofType(this.crudItemsActions.create),
        pluck('crudItem'),
        concatMap((crudItem) => of(crudItem).pipe(withLatestFrom(this.store.select(crudItemState.getLastLazyLoadEvent)))),
        switchMap(([crudItem, event]) => {
          return this.crudItemDas.post({ item: crudItem }).pipe(
            map(() => {
              this.biaMessageService.showAddSuccess();
              if (useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return this.crudItemsActions.loadAllByPost({ event: <LazyLoadEvent>event });
              }
            }),
            catchError((err) => {
              this.biaMessageService.showError();
              return of(this.crudItemsActions.failure({ error: err }));
            })
          );
        })
      )
    );
    createEffect(() =>
      this.actions$.pipe(
        ofType(this.crudItemsActions.update),
        pluck('crudItem'),
        concatMap((crudItem) => of(crudItem).pipe(withLatestFrom(this.store.select(crudItemState.getLastLazyLoadEvent)))),
        switchMap(([crudItem, event]) => {
          return this.crudItemDas.put({ item: crudItem, id: crudItem.id }).pipe(
            map(() => {
              this.biaMessageService.showUpdateSuccess();
              if (useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return this.crudItemsActions.loadAllByPost({ event: <LazyLoadEvent>event });
              }
            }),
            catchError((err) => {
              this.biaMessageService.showError();
              return of(this.crudItemsActions.failure({ error: err }));
            })
          );
        })
      )
    );
    // destroy
    createEffect(() =>
      this.actions$.pipe(
        ofType(this.crudItemsActions.remove),
        pluck('id'),
        concatMap((id: number) => of(id).pipe(withLatestFrom(this.store.select(crudItemState.getLastLazyLoadEvent)))),
        switchMap(([id, event]) => {
          return this.crudItemDas.delete({ id: id }).pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return this.crudItemsActions.loadAllByPost({ event: <LazyLoadEvent>event });
              }
            }),
            catchError((err) => {
              this.biaMessageService.showError();
              return of(this.crudItemsActions.failure({ error: err }));
            })
          );
        })
      )
    );

    // multiRemove
    createEffect(() =>
      this.actions$.pipe(
        ofType(this.crudItemsActions.multiRemove),
        pluck('ids'),
        concatMap((ids: number[]) => of(ids).pipe(withLatestFrom(this.store.select(crudItemState.getLastLazyLoadEvent)))),
        switchMap(([ids, event]) => {
          return this.crudItemDas.deletes({ ids: ids }).pipe(
            map(() => {
              this.biaMessageService.showDeleteSuccess();
              if (useSignalR) {
                return biaSuccessWaitRefreshSignalR();
              } else {
                return this.crudItemsActions.loadAllByPost({ event: <LazyLoadEvent>event });
              }
            }),
            catchError((err) => {
              this.biaMessageService.showError();
              return of(this.crudItemsActions.failure({ error: err }));
            })
          );
        })
    )
  );

  }
}
