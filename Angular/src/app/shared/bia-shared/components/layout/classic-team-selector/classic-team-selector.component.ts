import { Component, ChangeDetectionStrategy, Input, OnDestroy, Inject, OnInit } from '@angular/core';
import { BiaClassicLayoutService } from '../classic-layout/bia-classic-layout.service';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, Observable } from 'rxjs';
import { RoleMode } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { RoleDto } from '../../../model/role';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { allEnvironments } from 'src/environments/all-environments';
import { APP_BASE_HREF } from '@angular/common';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { DomainTeamsActions } from 'src/app/domains/bia-domains/team/store/teams-actions';
import { getAllTeamsOfType } from 'src/app/domains/bia-domains/team/store/team.state';
import { Team } from 'src/app/domains/bia-domains/team/model/team';

@Component({
  selector: 'bia-classic-team-selector',
  templateUrl: './classic-team-selector.component.html',
  styleUrls: ['./classic-team-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ClassicTeamSelectorComponent implements OnInit, OnDestroy {
  @Input() teamTypeId: number;

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

  private sub = new Subscription();


  unreadNotificationCount$: Observable<number>;

  constructor(
    public layoutService: BiaClassicLayoutService,
    public auth: AuthService,
    public translateService: TranslateService,
    public biaTranslationService: BiaTranslationService,
    private authService: AuthService,
    private store: Store<AppState>,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {
  }

  ngOnInit() {
    this.singleRoleMode = allEnvironments.teams.find(t => t.teamTypeId == this.teamTypeId && t.roleMode == RoleMode.SingleRole) != undefined;
    this.multiRoleMode = allEnvironments.teams.find(t => t.teamTypeId == this.teamTypeId && t.roleMode == RoleMode.MultiRoles) != undefined;
    this.teams$ = this.store.select(getAllTeamsOfType(this.teamTypeId));
    this.sub.add(
      this.biaTranslationService.languageId$.subscribe(languageId => {
        if (languageId) {
          this.languageId = languageId;
        }
      })
    );
    this.sub.add(
      this.teams$.subscribe(teams => {
        this.teams = teams
        this.initDropdownTeam();
        this.initDropdownRole();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onTeamChange() {
    this.authService.changeCurrentTeamId(this.teamTypeId, this.currentTeam.id);
    location.assign(this.baseHref);
  }

  onSetDefaultTeam() {
    this.store.dispatch(DomainTeamsActions.setDefaultTeam({ teamTypeId: this.teamTypeId, teamId: this.currentTeam.id }));
  }

  private initDropdownTeam() {
    this.displayTeamList = false;
    let currentTeamId = this.authService.getUncryptedToken()?.userData?.currentTeams?.find(t => t.teamTypeId == this.teamTypeId)?.teamId;
    let defaultTeamId = this.teams.find(t => t.isDefault)?.id;
    if (currentTeamId && currentTeamId > 0 && this.teams?.length > 1) {
      this.currentTeam = this.teams.filter((x) => x.id === currentTeamId)[0];
      this.displayTeamList = true;
      if (defaultTeamId) {
        this.defaultTeamId = defaultTeamId;
      }
    }
  }

  onRolesChange() {
    if (this.singleRoleMode) {
      if (this.currentRole) this.currentRoles = [this.currentRole];
    }

    this.authService.changeCurrentRoleIds(this.teamTypeId, this.currentTeam.id, this.currentRoles.map(r => r.id));
    location.assign(this.baseHref);
  }

  onSetDefaultRoles() {
    this.store.dispatch(DomainTeamsActions.setDefaultRoles({ teamId: this.currentTeam.id, roleIds: this.currentRoles.map(r => r.id) }));
  }

  isDefaultRoles(): boolean {
    return (this.defaultRoleIds?.sort().toString() === this.currentRoles?.map(r => r.id).sort().toString());
  }

  private initDropdownRole() {
    this.displayRoleList = false;
    this.displayRoleMultiSelect = false;
    if (this.singleRoleMode || this.multiRoleMode) {
      let currentRoleIds = this.authService.getUncryptedToken()?.userData?.currentTeams?.find(t => t.teamTypeId == this.teamTypeId)?.currentRoleIds;
      let roles = this.teams.find(t => t.id == this.currentTeam.id)?.roles;
      let defaultRoleIds = roles?.filter(r => r.isDefault).map(r => r.id);
      if ((roles && (this.multiRoleMode || roles.length > 1))) {
        this.currentRoles = roles?.filter((x) => currentRoleIds?.includes(x.id));
        if (this.singleRoleMode) {
          this.displayRoleList = true;
          if (this.currentRoles.length === 1) this.currentRole = this.currentRoles[0];
          else this.currentRole = null;
          this.roles = roles;
          if (defaultRoleIds && defaultRoleIds.length === 1) {
            this.defaultRoleIds = defaultRoleIds
          }
        }
        if (this.multiRoleMode) {
          this.displayRoleMultiSelect = true;
          this.roles = roles;
          if (defaultRoleIds) {
            this.defaultRoleIds = defaultRoleIds
          }
        }
      }
    }
  }
}
