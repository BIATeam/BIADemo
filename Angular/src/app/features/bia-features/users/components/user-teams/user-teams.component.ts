import { Component, Input } from '@angular/core';
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
  @Input() teams: UserTeam[];
}
