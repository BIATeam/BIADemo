import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import {
  AuthService,
  BiaAppConstantsService,
  BiaPermission,
  CoreTeamsStore,
} from 'packages/bia-ng/core/public-api';
import { ViewType } from 'packages/bia-ng/models/enum/public-api';
import { CurrentTeamDto, Team } from 'packages/bia-ng/models/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { AccordionModule } from 'primeng/accordion';
import { ConfirmationService, SharedModule } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { Dialog } from 'primeng/dialog';
import { Observable, Subscription } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ViewTeamTableComponent } from '../../components/view-team-table/view-team-table.component';
import { ViewUserTableComponent } from '../../components/view-user-table/view-user-table.component';
import { AssignViewToTeam } from '../../model/assign-view-to-team';
import { DefaultView } from '../../model/default-view';
import { TeamDefaultView } from '../../model/team-default-view';
import { View } from '../../model/view';
import { ViewsStore } from '../../store/view.state';
import { ViewsActions } from '../../store/views-actions';

@Component({
  selector: 'bia-manage-views-dialog',
  templateUrl: './manage-views-dialog.component.html',
  styleUrls: ['./manage-views-dialog.component.scss'],
  providers: [ConfirmationService],
  imports: [
    Dialog,
    SharedModule,
    ViewUserTableComponent,
    FormsModule,
    ViewTeamTableComponent,
    ButtonDirective,
    ConfirmDialog,
    AsyncPipe,
    TranslateModule,
    CommonModule,
    AccordionModule,
  ],
})
export class ManageViewsDialogComponent implements OnInit, OnDestroy {
  display = false;
  @Input() tableStateKey: string;
  @Input() useViewTeamWithTypeId: number | null;
  protected sub = new Subscription();

  teams$: Observable<Team[]>;
  views$: Observable<View[]>;
  viewTeams$: Observable<View[]>;
  viewUsers$: Observable<View[]>;
  userViewSelected: View | undefined;
  teamViewSelected: View | undefined;
  teamSelected: Team;
  originTeam: CurrentTeamDto | undefined;

  canAddTeamView = false;
  canAddUserView = false;
  canUpdateUserView = false;
  canUpdateTeamView = false;
  canDeleteTeamView = false;
  canDeleteUserView = false;
  canSetDefaultUserView = false;
  canSetDefaultTeamView = false;
  canAssignTeamView = false;

  constructor(
    protected store: Store<BiaAppState>,
    protected authService: AuthService
  ) {}

  ngOnInit() {
    this.setPermissions();
    this.initDisplay();
    this.initTeams();
    this.initViews();
    this.initViewTeams();
    this.initViewUsers();
  }

  protected initDisplay() {
    this.sub.add(
      this.store
        .select(ViewsStore.getDisplayViewDialog)
        .subscribe(
          tableStateKeySelected =>
            (this.display = this.tableStateKey === tableStateKeySelected)
        )
    );
  }

  protected initViews() {
    this.views$ = this.store.pipe(
      select(ViewsStore.getAllViews),
      map(views => views.filter(view => view.tableId === this.tableStateKey))
    );
  }

  protected initViewTeams() {
    this.viewTeams$ = this.views$.pipe(
      map(
        views => views.filter(view => view.viewType === ViewType.Team) as View[]
      )
    );

    this.sub.add(
      this.authService.authInfo$.subscribe(authInfo => {
        const currentTeams = authInfo.decryptedToken.userData.currentTeams;
        this.originTeam = currentTeams.length > 0 ? currentTeams[0] : undefined;
      })
    );
  }

  protected initViewUsers() {
    const currentTeamId =
      this.useViewTeamWithTypeId === null
        ? -1
        : this.authService.getCurrentTeamId(this.useViewTeamWithTypeId);
    this.viewUsers$ = this.views$.pipe(
      map(views =>
        views.filter(
          view =>
            view.viewType === ViewType.User ||
            (view.viewType === ViewType.Team &&
              view.viewTeams.some(t => t.id === currentTeamId))
        )
      )
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  initTeams() {
    const currentTeamId =
      this.useViewTeamWithTypeId === null
        ? -1
        : this.authService.getCurrentTeamId(this.useViewTeamWithTypeId);
    this.teams$ = this.store.select(CoreTeamsStore.getAllTeams).pipe(
      map(teams => teams.filter(team => currentTeamId === team.id)),
      tap(teams => {
        if (teams.length === 1) {
          this.teamSelected = teams[0];
        }
      })
    );
  }

  onClose() {
    this.userViewSelected = <View>{};
    this.teamViewSelected = <View>{};
    this.store.dispatch(ViewsActions.closeViewDialog());
  }

  onAssignViewToTeam(dto: AssignViewToTeam) {
    this.store.dispatch(ViewsActions.assignViewToTeam(dto));
  }

  onDeleteUserView(viewId: number) {
    this.userViewSelected = <View>{};
    this.store.dispatch(ViewsActions.removeUserView({ id: viewId }));
  }

  onSetDefaultUserView(event: {
    viewId: number | undefined;
    isDefault: boolean;
  }) {
    if (event.viewId) {
      const defaultView: DefaultView = {
        id: event.viewId,
        isDefault: event.isDefault,
        tableId: this.tableStateKey,
      };
      this.store.dispatch(ViewsActions.setDefaultUserView(defaultView));
    }
  }

  onSetDefaultTeamView(event: {
    viewId: number | undefined;
    isDefault: boolean;
  }) {
    if (this.teamSelected && event.viewId) {
      const defaultView: TeamDefaultView = {
        id: event.viewId,
        isDefault: event.isDefault,
        tableId: this.tableStateKey,
        teamId: this.teamSelected.id,
      };
      this.store.dispatch(ViewsActions.setDefaultTeamView(defaultView));
    }
  }

  onSaveUserView(view: View) {
    if (view) {
      const json = this.getViewPreference();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;
        if (view.id > 0) {
          this.store.dispatch(ViewsActions.updateUserView(view));
        } else {
          this.store.dispatch(ViewsActions.addUserView(view));
        }
      }
    }
  }

  onSaveTeamView(view: View) {
    if (view && this.teamSelected) {
      const json = this.getViewPreference();
      if (json) {
        view.preference = json;
        view.tableId = this.tableStateKey;

        if (view.id > 0) {
          this.store.dispatch(ViewsActions.updateTeamView(view));
        } else {
          this.store.dispatch(ViewsActions.addTeamView(view));
        }
      }
    }
  }

  onUserViewSelected(view: View | undefined) {
    this.userViewSelected = view;
  }

  onTeamViewSelected(view: View | undefined) {
    this.teamViewSelected = view;
  }

  protected getViewPreference(): string | null {
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

  get canSetUserView(): boolean {
    return (
      this.canAddUserView ||
      this.canDeleteUserView ||
      this.canSetDefaultUserView ||
      this.canUpdateUserView
    );
  }

  get canSetTeamView(): boolean {
    return (
      this.canAddTeamView ||
      this.canSetDefaultTeamView ||
      this.canUpdateTeamView ||
      this.canAssignTeamView
    );
  }

  protected setPermissions() {
    if (this.useViewTeamWithTypeId !== null) {
      const teamTypeRightPrefix =
        BiaAppConstantsService.teamTypeRightPrefix.find(
          t => t.key === this.useViewTeamWithTypeId
        )?.value;
      this.canAddTeamView = this.authService.hasPermission(
        teamTypeRightPrefix + BiaPermission.View_AddTeamViewSuffix
      );
      this.canUpdateTeamView = this.authService.hasPermission(
        teamTypeRightPrefix + BiaPermission.View_UpdateTeamViewSuffix
      );
      this.canSetDefaultTeamView = this.authService.hasPermission(
        teamTypeRightPrefix + BiaPermission.View_SetDefaultTeamViewSuffix
      );
      this.canAssignTeamView = this.authService.hasPermission(
        teamTypeRightPrefix + BiaPermission.View_AssignToTeamSuffix
      );
      this.canDeleteTeamView = this.authService.hasPermission(
        BiaPermission.View_DeleteTeamView
      );
    }
    this.canAddUserView = this.authService.hasPermission(
      BiaPermission.View_AddUserView
    );
    this.canUpdateUserView = this.authService.hasPermission(
      BiaPermission.View_UpdateUserView
    );
    this.canDeleteUserView = this.authService.hasPermission(
      BiaPermission.View_DeleteUserView
    );
    this.canSetDefaultUserView = this.authService.hasPermission(
      BiaPermission.View_SetDefaultUserView
    );
  }
}
