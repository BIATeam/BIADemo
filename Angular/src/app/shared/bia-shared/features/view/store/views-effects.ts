import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  loadAllSuccess,
  loadAllView,
  removeUserView,
  setDefaultUserView,
  addUserView,
  addTeamView,
  setDefaultTeamView,
  removeTeamView,
  updateUserView,
  assignViewToTeam,
  updateTeamView,
  setViewSuccess,
} from './views-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { ViewDas } from '../services/view-das.service';
import { View } from '../model/view';
import { TeamViewDas } from '../services/team-view-das.service';
import { TeamDefaultView } from '../model/team-default-view';
import { TeamView } from '../model/team-view';
import { UserViewDas } from '../services/user-view-das.service';
import { DefaultView } from '../model/default-view';
import { AssignViewToTeam } from '../model/assign-view-to-team';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class ViewsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAllView) /* When action is dispatched */,
      switchMap(() => {
        return this.viewDas.getAll().pipe(
          map(views => loadAllSuccess({ views })),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  deleteUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(removeUserView) /* When action is dispatched */,
      map(x => x?.id),
      switchMap(id => {
        return this.userViewDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
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
        this.userViewDas.post({ item: view }).pipe(
          switchMap(viewAdded => {
            this.biaMessageService.showAddSuccess();
            return [setViewSuccess(viewAdded), loadAllView()];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
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
        this.userViewDas.put({ item: view, id: view.id }).pipe(
          switchMap(viewUpdated => {
            this.biaMessageService.showUpdateSuccess();
            return [setViewSuccess(viewUpdated), loadAllView()];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  updateTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateTeamView),
      switchMap((view: TeamView) =>
        this.teamViewDas.put({ item: view, id: view.id }).pipe(
          switchMap(viewUpdated => {
            this.biaMessageService.showUpdateSuccess();
            return [setViewSuccess(viewUpdated), loadAllView()];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
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
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  deleteTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(removeTeamView) /* When action is dispatched */,
      map(x => x?.id),
      switchMap(id => {
        return this.teamViewDas.delete({ id: id }).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  addTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(addTeamView),
      switchMap((view: TeamView) =>
        this.teamViewDas.post({ item: view }).pipe(
          switchMap(viewAdded => {
            this.biaMessageService.showAddSuccess();
            return [setViewSuccess(viewAdded), loadAllView()];
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  setDefaultTeamView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultTeamView),
      switchMap((action: TeamDefaultView) => {
        return this.teamViewDas.setDefaultView(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  assignViewToTeam$ = createEffect(() =>
    this.actions$.pipe(
      ofType(assignViewToTeam),
      switchMap((action: AssignViewToTeam) => {
        return this.viewDas.assignViewToTeam(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAllView();
          }),
          catchError(err => {
            this.biaMessageService.showErrorHttpResponse(err);
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(
    private actions$: Actions,
    private viewDas: ViewDas,
    private teamViewDas: TeamViewDas,
    private userViewDas: UserViewDas,
    private biaMessageService: BiaMessageService
  ) {}
}
