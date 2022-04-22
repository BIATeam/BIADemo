import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'app-background-task-read-only',
  templateUrl: './background-task-read-only.component.html',
  styleUrls: ['./background-task-read-only.component.scss']
})
export class BackgroundTaskReadOnlyComponent {
  @HostBinding('class.bia-flex') flex = true;

  constructor() {
  }
}
