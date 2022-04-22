import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'app-background-task-admin',
  templateUrl: './background-task-admin.component.html',
  styleUrls: ['./background-task-admin.component.scss']
})
export class BackgroundTaskAdminComponent {
  @HostBinding('class.bia-flex') flex = true;

  constructor() {
  }
}
