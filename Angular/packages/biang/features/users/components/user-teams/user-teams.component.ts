import { Component, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'biang/core';
import { LoginParamDto } from 'biang/models';
import { TeamTypeId } from 'biang/models/enum';
import { Chip } from 'primeng/chip';
import { Tooltip } from 'primeng/tooltip';
import { UserTeam } from '../../model/user-team';

@Component({
  selector: 'bia-user-teams',
  templateUrl: './user-teams.component.html',
  styleUrls: ['./user-teams.component.scss'],
  imports: [Chip, Tooltip],
})
export class UserTeamsComponent {
  @Input() userTeams: UserTeam[];
  protected loginParam: LoginParamDto;

  constructor(
    protected translateService: TranslateService,
    protected authService: AuthService
  ) {
    this.loginParam = this.authService.getLoginParameters();
  }

  public getTeamTypeName(teamTypeId: number): string | undefined {
    const team = this.loginParam.teamsConfig.find(
      t => t.teamTypeId === teamTypeId
    );
    if (!team) {
      return undefined;
    }

    return team.label
      ? this.translateService.instant(team.label)
      : TeamTypeId[teamTypeId];
  }
}
