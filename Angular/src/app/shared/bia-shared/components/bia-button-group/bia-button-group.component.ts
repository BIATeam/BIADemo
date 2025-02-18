import { Component, Input, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'bia-button-group',
  templateUrl: './bia-button-group.component.html',
  styleUrl: './bia-button-group.component.scss',
})
export class BiaButtonGroupComponent {
  @ViewChild('menubar') menubar: any;
  @Input() buttons: ButtonGroupItem[];

  onMenubarClick() {
    this.setMenubarsubZIndex();
  }

  setMenubarsubZIndex() {
    const menubarsub = this.menubar.el?.nativeElement?.querySelector(
      'p-menubarsub'
    ) as HTMLElement;
    if (
      menubarsub &&
      menubarsub.attributes.getNamedItem('ng-reflect-mobile-active')?.value ===
        'true'
    ) {
      menubarsub.style.zIndex = '998';
    }
  }
}

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface ButtonGroupItem extends MenuItem {}
