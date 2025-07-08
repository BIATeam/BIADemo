import { NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { Notification } from 'src/app/domains/bia-domains/notification/model/notification';
import { IsNotCurrentTeamPipe } from './is-not-current-team/is-not-current-team.pipe';
import { TeamListPipe } from './team-list/team-list.pipe';

@Component({
  selector: 'bia-notification-team-warning',
  templateUrl: './notification-team-warning.component.html',
  styleUrls: ['./notification-team-warning.component.scss'],
  imports: [NgIf, TranslateModule, IsNotCurrentTeamPipe, TeamListPipe],
})
export class NotificationTeamWarningComponent {
  @Input() notification: Notification;
}
