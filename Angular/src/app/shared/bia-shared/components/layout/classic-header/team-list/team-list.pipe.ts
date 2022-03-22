import { Pipe, PipeTransform } from '@angular/core';
import { Notification } from 'src/app/domains/notification/model/notification';

@Pipe({
  name: 'teamList'
})
export class TeamListPipe implements PipeTransform {
  transform(notification: Notification): any {
    if (!notification?.notifiedTeams) {
      return '';
    }

    return notification.notifiedTeams.reduce((s, team) => s ? (s + ', ' + team.display) : team.display, '');
  }
}
