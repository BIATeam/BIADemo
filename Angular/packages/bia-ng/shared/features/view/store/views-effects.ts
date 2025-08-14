import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { BiaMessageService } from 'packages/bia-ng/core/public-api';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { AssignViewToTeam } from '../model/assign-view-to-team';
import { DefaultView } from '../model/default-view';
import { TeamDefaultView } from '../model/team-default-view';
import { TeamView } from '../model/team-view';
import { View } from '../model/view';
import { TeamViewDas } from '../services/team-view-das.service';
import { UserViewDas } from '../services/user-view-das.service';
import { ViewDas } from '../services/view-das.service';
import { ViewsActions } from './views-actions';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class ViewsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.loadAllView) /* When action is dispatched */,
      switchMap(() => {
        return this.viewDas.getAll().pipe(
          map(views => ViewsActions.loadAllSuccess({ views })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  deleteUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.removeUserView) /* When action is dispatched */,
      map(x => x?.id),
      switchMap(id => {
        return this.userViewDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return ViewsActions.loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  addUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.addUserView),
      switchMap((view: View) =>
        this.userViewDas.post({ item: view }).pipe(
          switchMap(viewAdded => {
            this.biaMessageService.showAddSuccess();
            return [
              ViewsActions.setViewSuccess(viewAdded),
              ViewsActions.loadAllView(),
            ];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  updateUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.updateUserView),
      switchMap((view: View) =>
        this.userViewDas.put({ item: view, id: view.id }).pipe(
          switchMap(viewUpdated => {
            this.biaMessageService.showUpdateSuccess();
            return [
              ViewsActions.setViewSuccess(viewUpdated),
              ViewsActions.loadAllView(),
            ];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  updateTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.updateTeamView),
      switchMap((view: TeamView) =>
        this.teamViewDas.put({ item: view, id: view.id }).pipe(
          switchMap(viewUpdated => {
            this.biaMessageService.showUpdateSuccess();
            return [
              ViewsActions.setViewSuccess(viewUpdated),
              ViewsActions.loadAllView(),
            ];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  setDefaultUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.setDefaultUserView),
      switchMap((action: DefaultView) => {
        return this.userViewDas.setDefaultView(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return ViewsActions.loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  deleteTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.removeTeamView) /* When action is dispatched */,
      map(x => x?.id),
      switchMap(id => {
        return this.teamViewDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return ViewsActions.loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  addTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.addTeamView),
      switchMap((view: TeamView) =>
        this.teamViewDas.post({ item: view }).pipe(
          switchMap(viewAdded => {
            this.biaMessageService.showAddSuccess();
            return [
              ViewsActions.setViewSuccess(viewAdded),
              ViewsActions.loadAllView(),
            ];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        )
      )
    )
  );

  setDefaultTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.setDefaultTeamView),
      switchMap((action: TeamDefaultView) => {
        return this.teamViewDas.setDefaultView(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return ViewsActions.loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  assignViewToTeam$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ViewsActions.assignViewToTeam),
      switchMap((action: AssignViewToTeam) => {
        return this.viewDas.assignViewToTeam(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return ViewsActions.loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(ViewsActions.failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    protected actions$: Actions,
    protected viewDas: ViewDas,
    protected teamViewDas: TeamViewDas,
    protected userViewDas: UserViewDas,
    protected biaMessageService: BiaMessageService
  ) {}
}
