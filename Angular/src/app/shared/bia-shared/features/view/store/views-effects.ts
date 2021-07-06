import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap, pluck } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadAllSuccess,
  loadAllView,
  removeUserView,
  setDefaultUserView,
  addUserView,
  addSiteView,
  setDefaultSiteView,
  removeSiteView,
  updateUserView,
  assignViewToSite,
  updateSiteView,
  setViewSuccess
} from './views-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { ViewDas } from '../services/view-das.service';
import { View } from '../model/view';
import { SiteViewDas } from '../services/site-view-das.service';
import { SiteDefaultView } from '../model/site-default-view';
import { SiteView } from '../model/site-view';
import { UserViewDas } from '../services/user-view-das.service';
import { DefaultView } from '../model/default-view';
import { AssignViewToSite } from '../model/assign-view-to-site';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class ViewsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllView) /* When action is dispatched */,
      switchMap((action) => {
        return this.viewDas.getAll().pipe(
          map((views) => loadAllSuccess({ views })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  deleteUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(removeUserView) /* When action is dispatched */,
      pluck('id'),
      switchMap((id) => {
        return this.userViewDas.delete(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllView();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  addUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(addUserView),
      switchMap((view: View) =>
        this.userViewDas.post(view).pipe(
          switchMap((viewAdded) => {
            this.biaMessageService.showAddSuccess();
            return [setViewSuccess(viewAdded), loadAllView()];
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  updateUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateUserView),
      switchMap((view: View) =>
        this.userViewDas.put(view, view.id).pipe(
          switchMap((viewUpdated) => {
            this.biaMessageService.showUpdateSuccess();
            return [setViewSuccess(viewUpdated), loadAllView()];
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  updateSiteView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateSiteView),
      switchMap((view: SiteView) =>
        this.siteViewDas.put(view, view.id).pipe(
          switchMap((viewUpdated) => {
            this.biaMessageService.showUpdateSuccess();
            return [setViewSuccess(viewUpdated), loadAllView()];
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  setDefaultUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultUserView),
      switchMap((action: DefaultView) => {
        return this.userViewDas.setDefaultView(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllView();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  deleteSiteView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(removeSiteView) /* When action is dispatched */,
      pluck('id'),
      switchMap((id) => {
        return this.siteViewDas.delete(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllView();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  addSiteView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(addSiteView),
      switchMap((view: SiteView) =>
        this.siteViewDas.post(view).pipe(
          switchMap((viewAdded) => {
            this.biaMessageService.showAddSuccess();
            return [setViewSuccess(viewAdded), loadAllView()];
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  setDefaultSiteView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultSiteView),
      switchMap((action: SiteDefaultView) => {
        return this.siteViewDas.setDefaultView(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllView();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  assignViewToSite$ = createEffect(() =>
    this.actions$.pipe(
      ofType(assignViewToSite),
      switchMap((action: AssignViewToSite) => {
        return this.viewDas.assignViewToSite(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllView();
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
    private viewDas: ViewDas,
    private siteViewDas: SiteViewDas,
    private userViewDas: UserViewDas,
    private biaMessageService: BiaMessageService
  ) {}
}
