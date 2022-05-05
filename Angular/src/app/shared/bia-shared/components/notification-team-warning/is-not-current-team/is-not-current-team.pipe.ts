import { Pipe, PipeTransform } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Notification } from 'src/app/domains/bia-domains/notification/model/notification';

@Pipe({
  name: 'isNotCurrentTeam'
})
export class IsNotCurrentTeamPipe implements PipeTransform {
  constructor(public auth: AuthService) { }

  transform(notification: Notification): any {
    if (!notification?.data.teams) {
      return false;
    }
    return !notification.data.teams?.some(team => this.auth.getCurrentTeamId(team.teamTypeId) === team.team.id);
  }
}
