import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { IsNotCurrentTeamPipe } from './is-not-current-team/is-not-current-team.pipe';
import { NotificationTeamWarningComponent } from './notification-team-warning.component';
import { TeamListPipe } from './team-list/team-list.pipe';

@NgModule({
  declarations: [
    NotificationTeamWarningComponent,
    IsNotCurrentTeamPipe,
    TeamListPipe,
  ],
  exports: [
    NotificationTeamWarningComponent,
    IsNotCurrentTeamPipe,
    TeamListPipe,
  ],
  imports: [CommonModule, TranslateModule],
})
export class NotificationTeamWarningModule {}
