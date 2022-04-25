import { Component, Input } from '@angular/core';
import { Notification } from 'src/app/domains/bia-domains/notification/model/notification';

@Component({
  selector: 'bia-notification-team-warning',
  templateUrl: './notification-team-warning.component.html',
  styleUrls: ['./notification-team-warning.component.scss']
})
export class NotificationTeamWarningComponent {
  @Input() notification: Notification;

  constructor() { }
}
