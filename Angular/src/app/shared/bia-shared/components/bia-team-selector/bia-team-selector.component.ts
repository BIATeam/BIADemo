import { APP_BASE_HREF, NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MultiSelect } from 'primeng/multiselect';
import { Select } from 'primeng/select';
import { Tooltip } from 'primeng/tooltip';
import { Observable, Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Team } from 'src/app/domains/bia-domains/team/model/team';
import { getAllTeamsOfType } from 'src/app/domains/bia-domains/team/store/team.state';
import { DomainTeamsActions } from 'src/app/domains/bia-domains/team/store/teams-actions';
import { RoleMode } from 'src/app/shared/constants';
import { AppState } from 'src/app/store/state';
import { AuthInfo, TeamConfigDto } from '../../model/auth-info';
import { RoleDto } from '../../model/role';
import { BiaLayoutService } from '../layout/services/layout.service';

@Component({
  selector: 'bia-team-selector',
  templateUrl: './bia-team-selector.component.html',
  styleUrls: ['./bia-team-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [NgIf, Select, FormsModule, Tooltip, MultiSelect, TranslateModule],
})
export class BiaTeamSelectorComponent implements OnInit, OnDestroy {
  @Input() teamType: TeamConfigDto;

  displayTeamList = false;
  defaultTeamId = 0;
  currentTeam: Team;
  teams: Team[];
  teams$: Observable<Team[]>;

  displayRoleList = false;
  displayRoleMultiSelect = false;
  defaultRoleIds: number[] = [];
  currentRole: RoleDto | null;
  currentRoles: RoleDto[];
  roles: RoleDto[];

  languageId: number;
  singleRoleMode: boolean;
  multiRoleMode: boolean;
  canClear: boolean | undefined;

  protected sub = new Subscription();

  unreadNotificationCount$: Observable<number>;

  constructor(
    public layoutService: BiaLayoutService,
    public auth: AuthService,
    public translateService: TranslateService,
    public biaTranslationService: BiaTranslationService,
    protected authService: AuthService,
    protected store: Store<AppState>,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {}

  ngOnInit() {
    this.canClear = this.authService
      .getLoginParameters()
      .teamsConfig.find(
        t => t.teamTypeId === this.teamType.teamTypeId
      )?.teamSelectionCanBeEmpty;
    this.singleRoleMode =
      this.authService
        .getLoginParameters()
        .teamsConfig.find(
          t =>
            t.teamTypeId === this.teamType.teamTypeId &&
            t.roleMode === RoleMode.SingleRole
        ) !== undefined;
    this.multiRoleMode =
      this.authService
        .getLoginParameters()
        .teamsConfig.find(
          t =>
            t.teamTypeId === this.teamType.teamTypeId &&
            t.roleMode === RoleMode.MultiRoles
        ) !== undefined;
    this.teams$ = this.store.select(
      getAllTeamsOfType(this.teamType.teamTypeId)
    );
    this.sub.add(
      this.biaTranslationService.languageId$.subscribe(languageId => {
        if (languageId) {
          this.languageId = languageId;
        }
      })
    );
    this.sub.add(
      this.teams$.subscribe(teams => {
        this.teams = this.sortTeams(teams);
        this.initDropdownTeam();
        this.initDropdownRole();
      })
    );
    this.sub.add(
      this.authService.authInfo$.subscribe((authInfo: AuthInfo) => {
        if (authInfo && authInfo.token !== '') {
          this.initDropdownTeam();
          this.initDropdownRole();
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onTeamChange() {
    this.authService.changeCurrentTeamId(
      this.teamType.teamTypeId,
      this.currentTeam ? this.currentTeam.id : 0
    );
    this.authService.clearSessionExceptLoginInfos();
    location.reload();
  }

  onSetDefaultTeam() {
    if (this.currentTeam.id) {
      this.store.dispatch(
        DomainTeamsActions.setDefaultTeam({
          teamTypeId: this.teamType.teamTypeId,
          teamId: this.currentTeam.id,
        })
      );
    }
  }

  onResetDefaultTeam() {
    this.store.dispatch(
      DomainTeamsActions.resetDefaultTeam({
        teamTypeId: this.teamType.teamTypeId,
      })
    );
  }

  protected initDropdownTeam() {
    this.displayTeamList = false;
    const currentTeamId = this.authService
      .getDecryptedToken()
      ?.userData?.currentTeams?.find(
        t => t.teamTypeId === this.teamType.teamTypeId
      )?.teamId;
    const defaultTeamId = this.teams.find(t => t.isDefault)?.id;
    if (currentTeamId && currentTeamId > 0) {
      this.currentTeam = this.teams.filter(x => x.id === currentTeamId)[0];
    }
    if (
      this.teams?.length > 1 ||
      (this.teams?.length === 1 && this.teamType.displayOne === true) ||
      this.teamType.displayAlways
    ) {
      this.displayTeamList = true;
    }
    if (defaultTeamId) {
      this.defaultTeamId = defaultTeamId;
    }
  }

  onRolesChange() {
    if (!this.currentRoles) {
      this.currentRoles = [];
    }

    if (this.singleRoleMode && this.currentRole) {
      this.currentRoles = [this.currentRole];
    }

    this.authService.changeCurrentRoleIds(
      this.teamType.teamTypeId,
      this.currentTeam.id,
      this.currentRoles.map(r => r.id)
    );
    location.reload();
  }

  onSetDefaultRoles() {
    this.store.dispatch(
      DomainTeamsActions.setDefaultRoles({
        teamId: this.currentTeam.id,
        roleIds: this.currentRoles.map(r => r.id),
      })
    );
  }

  onResetDefaultRoles() {
    this.store.dispatch(
      DomainTeamsActions.resetDefaultRoles({
        teamId: this.currentTeam.id,
      })
    );
  }

  isDefaultRoles(): boolean {
    return (
      this.defaultRoleIds?.sort().toString() ===
      this.currentRoles
        ?.map(r => r.id)
        .sort()
        .toString()
    );
  }

  protected initDropdownRole() {
    this.displayRoleList = false;
    this.displayRoleMultiSelect = false;
    this.defaultRoleIds = [];
    if (this.singleRoleMode || this.multiRoleMode) {
      const currentRoleIds = this.authService
        .getDecryptedToken()
        ?.userData?.currentTeams?.find(
          t => t.teamTypeId === this.teamType.teamTypeId
        )?.currentRoleIds;
      let roles = this.teams.find(t => t.id === this.currentTeam?.id)?.roles;
      roles = roles ? [...roles] : roles;
      const defaultRoleIds = roles?.filter(r => r.isDefault).map(r => r.id);
      if (roles && (this.multiRoleMode || roles.length > 1)) {
        this.currentRoles = roles?.filter(x => currentRoleIds?.includes(x.id));
        if (this.singleRoleMode) {
          this.displayRoleList = true;
          if (this.currentRoles.length === 1)
            this.currentRole = this.currentRoles[0];
          else this.currentRole = null;
          this.roles = this.sortRoles(roles);
          if (defaultRoleIds && defaultRoleIds.length === 1) {
            this.defaultRoleIds = defaultRoleIds;
          }
        }
        if (this.multiRoleMode) {
          this.displayRoleMultiSelect = true;
          this.roles = this.sortRoles(roles);
          if (defaultRoleIds) {
            this.defaultRoleIds = defaultRoleIds;
          }
        }
      }
    }
  }

  protected sortTeams(teams: Team[]): Team[] {
    return teams?.sort((a, b) => {
      return a?.title?.localeCompare(b?.title);
    });
  }

  protected sortRoles(roles: RoleDto[]): RoleDto[] {
    return roles?.sort((a, b) => {
      return a?.display?.localeCompare(b?.display);
    });
  }
}
