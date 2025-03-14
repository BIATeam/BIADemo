import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';

import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'bia-scrolling-notification',
  templateUrl: './scrolling-notification.component.html',
  styleUrls: ['./scrolling-notification.component.scss'],
  animations: [
    trigger('scroll', [
      state('on', style({ left: '100vw' })),
      transition('* => *', [
        style({ left: '100vw' }),
        animate(20000, style({ left: '-24rem' })),
      ]),
    ]),
  ],
})
export class BiaScrollingNotificationComponent {
  @HostBinding('class') classes = 'scrolling-notification';

  @Input() notification: string | null;

  state = 0;

  scrollDone() {
    this.state++;
  }
}
