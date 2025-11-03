import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'bia-scrolling-notification',
  templateUrl: './scrolling-notification.component.html',
  styleUrls: ['./scrolling-notification.component.scss'],
})
export class BiaScrollingNotificationComponent {
  @HostBinding('class') classes = 'scrolling-notification';

  @Input() notification: string | null;

  state = 0;

  scrollDone() {
    this.state++;
  }
}
