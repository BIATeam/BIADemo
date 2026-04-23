import { Pipe, PipeTransform } from '@angular/core';
import { AuthService, Notification } from 'packages/bia-ng/core/public-api';

@Pipe({ name: 'isNotCurrentTeam' })
export class IsNotCurrentTeamPipe implements PipeTransform {
  constructor(public auth: AuthService) {}

  transform(notification: Notification): any {
    if (!notification?.data?.teams) {
      return false;
    }
    return !notification.data.teams?.some(
      team => this.auth.getCurrentTeamId(team.teamTypeId) === team.team.id
    );
  }
}
