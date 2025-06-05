import { Component, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Chip } from 'primeng/chip';
import { Tooltip } from 'primeng/tooltip';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { LoginParamDto } from 'src/app/shared/bia-shared/model/auth-info';
import { TeamTypeId } from 'src/app/shared/constants';
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
