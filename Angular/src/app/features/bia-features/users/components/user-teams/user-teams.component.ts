import { Component, Input } from '@angular/core';
import { Chip } from 'primeng/chip';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

@Component({
  selector: 'bia-user-teams',
  templateUrl: './user-teams.component.html',
  styleUrls: ['./user-teams.component.scss'],
  imports: [Chip],
})
export class UserTeamsComponent {
  @Input() teams: OptionDto[];
}
