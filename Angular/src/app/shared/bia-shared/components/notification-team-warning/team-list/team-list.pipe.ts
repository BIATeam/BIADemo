import { Pipe, PipeTransform } from '@angular/core';
import { Notification } from 'src/app/domains/bia-domains/notification/model/notification';

@Pipe({
  name: 'teamList'
})
export class TeamListPipe implements PipeTransform {
  transform(notification: Notification): any {
    if (!notification?.data.teams) {
      return '';
    }

    return notification.data.teams.reduce((s, team) => s ? (s + ', ' + team.display) : team.display, '');
  }
}
