import { Component, ChangeDetectionStrategy, Input, OnDestroy, Inject, OnInit } from '@angular/core';
import { BiaClassicLayoutService } from '../classic-layout/bia-classic-layout.service';
import { MenuItem } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, Observable } from 'rxjs';
import { RoleMode } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { OptionDto } from '../../../model/option-dto';
import { UserData } from '../../../model/auth-info';
import { RoleDto } from '../../../model/role';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { allEnvironments } from 'src/environments/allEnvironments';
import { APP_BASE_HREF } from '@angular/common';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { setDefaultRoles, setDefaultTeam } from 'src/app/domains/site/store/sites-actions';

@Component({
  selector: 'bia-classic-team-selector',
  templateUrl: './classic-team-selector.component.html',
  styleUrls: ['./classic-team-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ClassicTeamSelectorComponent implements OnInit, OnDestroy {
  @Input() teamTypeId: number;
  currentTeam: OptionDto;
  currentRole: RoleDto | null;
  currentRoles: RoleDto[];
  _userData: UserData;
  get userData(): UserData {
    return this._userData;
  }
  @Input()
  set userData(value: UserData) {
    this._userData = value;

  }

  displayTeamList = false;
  defaultTeamId = 0;
  teams:  OptionDto[];
  displayRoleList = false;
  defaultRoleIds: number[] = [];
  roles:  RoleDto[];

  cssClassEnv: string;
  languageId: number;
  singleRoleMode: boolean;

  private sub = new Subscription();

  topBarMenuItems: any; // MenuItem[]; // bug v9 primeNG
  navMenuItems: MenuItem[];
  appIcon$: Observable<string>;

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
    this.sub.add(
      this.biaTranslationService.languageId$.subscribe(languageId => {
        if (languageId) {
          this.languageId = languageId;
        }
      })
    );
    this.singleRoleMode = allEnvironments.teams.find(t => t.teamTypeId == this.teamTypeId && t.roleMode == RoleMode.SingleRole) != undefined
    this.initDropdownTeam();
    this.initDropdownRole();
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onTeamChange() {
    this.authService.setCurrentTeamId(this.teamTypeId, this.currentTeam.id);
    location.assign(this.baseHref);
  }

  onSetDefaultTeam() {
    this.store.dispatch(setDefaultTeam({teamTypeId: this.teamTypeId, teamId:this.currentTeam.id}));
    this.defaultTeamId = this.currentTeam.id;
  }

  private initDropdownTeam() {
    this.displayTeamList = false;
    let currentTeamId = this.userData.currentTeams.find(t => t.teamTypeId == this.teamTypeId)?.currentTeamId;
    let teams = this.userData.currentTeams.find(t => t.teamTypeId == this.teamTypeId)?.teams;
    let defaultTeamId = this.userData.currentTeams.find(t => t.teamTypeId == this.teamTypeId)?.defaultTeamId;
    if (currentTeamId && currentTeamId != undefined && currentTeamId > 0 && 
      teams && teams != undefined && teams.length > 1) {
      this.currentTeam = teams.filter((x) => x.id === currentTeamId)[0];
      this.displayTeamList = true;
      this.teams = teams;
      if (defaultTeamId)
      {
        this.defaultTeamId = defaultTeamId;
      }
    }
  }

  onRolesChange() {
    let roleIds: number[] = []
    if (this.singleRoleMode){
      roleIds = [this.currentRole!.id]
    }
    else
    {
      roleIds = this.currentRoles.map(r => r.id)
    }
    this.authService.setCurrentRoleIds(this.currentTeam.id, roleIds);
    location.assign(this.baseHref);
  }

  onSetDefaultRoles() {
    let roleIds: number[] = []
    if (this.singleRoleMode){
      roleIds = [this.currentRole!.id]
    }
    else
    {
      roleIds = this.currentRoles.map(r => r.id)
    }
    this.store.dispatch(setDefaultRoles({teamId: this.currentTeam.id, roleIds: roleIds}));
    this.defaultRoleIds = this.currentRoles.map(r => r.id);
  }


  private initDropdownRole() {
    this.displayRoleList = false;
    if (this.singleRoleMode) {
      let currentRoleIds = this.userData.currentTeams.find(t => t.teamTypeId == this.teamTypeId)?.currentRoleIds;
      let defaultRoleIds = this.userData.currentTeams.find(t => t.teamTypeId == this.teamTypeId)?.defaultRoleIds;
      let roles = this.userData.currentTeams.find(t => t.teamTypeId == this.teamTypeId)?.roles;
      if (roles  && roles != undefined && roles.length > 1) {
        this.displayRoleList = true;
        this.currentRoles = roles.filter((x) => currentRoleIds?.includes(x.id));
        if (this.currentRoles.length ===1 ) this.currentRole = this.currentRoles[0];
        else this.currentRole = null;
        this.roles = roles;
        if (defaultRoleIds && defaultRoleIds.length === 1)
        {
          this.defaultRoleIds = defaultRoleIds
        }
      }
    }
  }
}
