import { NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { Tooltip } from 'primeng/tooltip';

@Component({
  selector: 'bia-buttons',
  templateUrl: './bia-buttons.component.html',
  styleUrls: ['./bia-buttons.component.scss'],
  imports: [NgIf, ButtonDirective, Tooltip],
})
export class BiaButtonsComponent {
  @Input() buttons: BiaButtonItem[];
}

export interface BiaButtonItem extends MenuItem {
  buttonClass?: string;
  order?: number;
  withThrottleClick?: boolean;
}
