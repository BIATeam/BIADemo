import { Component, Input } from '@angular/core';
import { BiaButtonAndMenuItem } from '@bia-team/bia-ng/models';
import { ButtonDirective } from 'primeng/button';
import { Divider } from 'primeng/divider';
import { TieredMenu } from 'primeng/tieredmenu';
import { Tooltip } from 'primeng/tooltip';
import { ThrottleEventDirective } from '../../directives/throttle-click.directive';

@Component({
  selector: 'bia-buttons',
  templateUrl: './bia-buttons.component.html',
  styleUrls: ['./bia-buttons.component.scss'],
  imports: [
    ButtonDirective,
    Tooltip,
    Divider,
    ThrottleEventDirective,
    TieredMenu,
  ],
})
export class BiaButtonsComponent {
  @Input() buttons: BiaButtonAndMenuItem[];
}
