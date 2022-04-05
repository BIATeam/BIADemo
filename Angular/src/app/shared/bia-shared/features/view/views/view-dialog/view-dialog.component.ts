import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Subscription, Observable } from 'rxjs';
import { View } from '../../model/view';
import { Store, select } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import {
  removeUserView,
  addUserView,
  addTeamView,
  removeTeamView,
  setDefaultUserView,
  setDefaultTeamView,
  closeViewDialog,
  updateUserView,
  assignViewToTeam,
  updateTeamView
} from '../../store/views-actions';
import { getAllViews, getDisplayViewDialog } from '../../store/view.state';
import { map, tap } from 'rxjs/operators';
import { TeamTypeId, ViewType } from 'src/app/shared/constants';
import { TeamView } from '../../model/team-view';
import { DefaultView } from '../../model/default-view';
import { TeamDefaultView } from '../../model/team-default-view';
import { Dialog } from 'primeng/dialog';
import { AssignViewToTeam } from '../../model/assign-view-to-team';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { Confirmation, ConfirmationService } from 'primeng/api';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { getAllTeams } from 'src/app/domains/team/store/team.state';
import { Team } from 'src/app/domains/team/model/team';

@Component({
  selector: 'bia-view-dialog',
  templateUrl: './view-dialog.component.html',
  styleUrls: ['./view-dialog.component.scss'],
  providers: [ConfirmationService]
})
export class ViewDialogComponent implements OnInit, OnDestroy {
  display = false;
  @Input() tableStateKey: string;
  @Input() useViewTeam: TeamTypeId;
  private sub = new Subscription();

  teams$: Observable<Team[]>;
  views$: Observable<View[]>;
  viewTeams$: Observable<View[]>;
  viewUsers$: Observable<View[]>;
  userViewSelected: View;
  teamViewSelected: View;
  teamSelected: Team;

  canAddTeamView = false;
  canAddUserView = false;
  canUpdateUserView = false;
  canUpdateTeamView = false;
  canDeleteUserView = false;
  canDeleteTeamView = false;
  canSetDefaultUserView = false;
  canSetDefaultTeamView = false;
  canAssignTeamView = false;

  constructor(
    private store: Store<AppState>,
    private authService: AuthService,
    private biaDialogService: BiaDialogService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.setPermissions();
    this.initDisplay();
    this.initTeams();
    this.initViews();
    this.initViewTeams();
    this.initViewUsers();
  }

  private initDisplay() {
    this.sub.add(
      this.store
        .select(getDisplayViewDialog)
        .subscribe((tableStateKeySelected) => (this.display = this.tableStateKey === tableStateKeySelected))
    );
  }

  private initViews() {
    this.views$ = this.store
      .pipe(select(getAllViews))
      .pipe(map((views) => views.filter((view) => view.tableId === this.tableStateKey)));
  }

  private initViewTeams() {
    this.viewTeams$ = this.views$.pipe(map((views) => views.filter((view) => view.viewType === ViewType.Team)));
  }

  private initViewUsers() {
    this.viewUsers$ = this.views$.pipe(map((views) => views.filter((view) => view.viewType === ViewType.User)));
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  initTeams() {
    let currentTeamId = this.authService.getCurrentTeamId(this.useViewTeam);
    this.teams$ = this.store.select(getAllTeams).pipe(
      map((teams) =>
        teams.filter(
          (team) => /*this.authService.getCurrentTeamId(this.useViewTeam) < 1 || */ team.id == currentTeamId
        )
      ),
      tap((teams) => {
        if (teams.length === 1) {
          this.teamSelected = teams[0];
        }
      })
    );
  }

  onClose() {
    this.userViewSelected = <View>{};
    this.teamViewSelected = <View>{};
    this.store.dispatch(closeViewDialog());
  }

  showDialogMaximized(dialog: Dialog) {
    dialog.maximize();
  }

  onAssignViewToTeam(dto: AssignViewToTeam) {
    this.store.dispatch(assignViewToTeam(dto));
  }

  onDeleteUserView(viewId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.userViewSelected = <View>{};
        this.store.dispatch(removeUserView({ id: viewId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onDeleteTeamView(viewId: number) {
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.teamViewSelected = <View>{};
        this.store.dispatch(removeTeamView({ id: viewId }));
      }
    };
    this.confirmationService.confirm(confirmation);
  }

  onSetDefaultUserView(event: { viewId: number; isDefault: boolean }) {
    const defaultView: DefaultView = { id: event.viewId, isDefault: event.isDefault, tableId: this.tableStateKey };
    this.store.dispatch(setDefaultUserView(defaultView));
  }

  onSetDefaultTeamView(event: { viewId: number; isDefault: boolean }) {
    if (this.teamSelected) {
      const defaultView: TeamDefaultView = {
        id: event.viewId,
        isDefault: event.isDefault,
        tableId: this.tableStateKey,
        teamId: this.teamSelected.id
      };
      this.store.dispatch(setDefaultTeamView(defaultView));
    }
  }

  onSaveUserView(view: View) {
    if (view) {
      const json = this.GetViewPreference();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;
        if (view.id > 0) {
          this.store.dispatch(updateUserView(view));
        } else {
          this.store.dispatch(addUserView(view));
        }
      }
    }
  }

  onSaveTeamView(view: TeamView) {
    if (view && this.teamSelected) {
      const json = this.GetViewPreference();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;
        view.teamId = this.teamSelected.id;
        if (view.id > 0) {
          this.store.dispatch(updateTeamView(view));
        } else {
          this.store.dispatch(addTeamView(view));
        }
      }
    }
  }

  onUserViewSelected(view: View) {
    this.userViewSelected = view;
  }

  onTeamViewSelected(view: View) {
    this.teamViewSelected = view;
  }

  private GetViewPreference(): string | null {
    let stateString = sessionStorage.getItem(this.tableStateKey);
    if (stateString) {
      const state = JSON.parse(stateString);
      if (!state.filters) {
        state.filters = {};
      }
      stateString = JSON.stringify(state);
    }

    return stateString;
  }

  canSetUserView() {
    return this.canAddUserView || this.canDeleteUserView || this.canSetDefaultUserView || this.canUpdateUserView;
  }

  canSetTeamView() {
    return (
      this.canAddTeamView ||
      this.canDeleteTeamView ||
      this.canSetDefaultTeamView ||
      this.canUpdateTeamView ||
      this.canAssignTeamView
    );
  }

  private setPermissions() {
    this.canAddTeamView = this.authService.hasPermission(Permission.View_AddTeamView);
    this.canAddUserView = this.authService.hasPermission(Permission.View_AddUserView);
    this.canUpdateUserView = this.authService.hasPermission(Permission.View_UpdateUserView);
    this.canUpdateTeamView = this.authService.hasPermission(Permission.View_UpdateTeamView);
    this.canDeleteUserView = this.authService.hasPermission(Permission.View_DeleteUserView);
    this.canDeleteTeamView = this.authService.hasPermission(Permission.View_DeleteTeamView);
    this.canSetDefaultUserView = this.authService.hasPermission(Permission.View_SetDefaultUserView);
    this.canSetDefaultTeamView = this.authService.hasPermission(Permission.View_SetDefaultTeamView);
    this.canAssignTeamView = this.authService.hasPermission(Permission.View_AssignToTeam);
  }
}
