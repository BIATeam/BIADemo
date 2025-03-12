import { animate, style, transition, trigger } from '@angular/animations';
import { NgFor, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { BiaUltimaMenuItemComponent } from '../menu-item/ultima-menu-item.component';

@Component({
  selector: 'bia-ultima-menu',
  templateUrl: './ultima-menu.component.html',
  animations: [
    trigger('menu', [
      transition('void => inline', [
        style({ height: 0 }),
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 1, height: '*' })
        ),
      ]),
      transition('inline => void', [
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 0, height: '0' })
        ),
      ]),
      transition('void => overlay', [
        style({ opacity: 0, transform: 'scaleY(0.8)' }),
        animate('.12s cubic-bezier(0, 0, 0.2, 1)'),
      ]),
      transition('overlay => void', [
        animate('.1s linear', style({ opacity: 0 })),
      ]),
    ]),
  ],
  imports: [NgFor, NgIf, BiaUltimaMenuItemComponent],
})
export class BiaUltimaMenuComponent {
  @Input() menuItems: MenuItem[] = [];
}
