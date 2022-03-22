import { Pipe, PipeTransform } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Notification } from 'src/app/domains/notification/model/notification';

@Pipe({
  name: 'isNotCurrentTeam'
})
export class IsNotCurrentTeamPipe implements PipeTransform {
  constructor(public auth: AuthService) { }

  transform(notification: Notification): any {
    if (!notification?.notifiedTeams) {
      return false;
    }

    return !notification.notifiedTeams?.some(team => this.auth.getCurrentTeamId(team.typeId) === team.id);
  }
}