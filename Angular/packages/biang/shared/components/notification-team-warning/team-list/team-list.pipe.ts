import { Pipe, PipeTransform } from '@angular/core';
import { Notification } from 'biang/core';

@Pipe({ name: 'teamList' })
export class TeamListPipe implements PipeTransform {
  transform(notification: Notification): any {
    if (!notification?.data?.teams) {
      return '';
    }

    return notification.data.teams.reduce(
      (s, notificationTeam) =>
        s
          ? s + ', ' + notificationTeam.team.display
          : notificationTeam.team.display,
      ''
    );
  }
}
