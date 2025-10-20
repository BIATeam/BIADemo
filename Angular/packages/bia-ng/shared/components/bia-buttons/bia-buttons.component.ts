import { Component, Input } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { Divider } from 'primeng/divider';
import { SplitButton } from 'primeng/splitbutton';
import { Tooltip } from 'primeng/tooltip';

@Component({
  selector: 'bia-buttons',
  templateUrl: './bia-buttons.component.html',
  styleUrls: ['./bia-buttons.component.scss'],
  imports: [ButtonDirective, Tooltip, SplitButton, Divider],
})
export class BiaButtonsComponent {
  @Input() buttons: BiaButtonItem[];
}

export interface BiaButtonItem extends MenuItem {
  labelAsTooltip?: boolean;
  buttonClass?: string;
  menuSeparator?: boolean;
  order?: number;
  withThrottleClick?: boolean;
}
