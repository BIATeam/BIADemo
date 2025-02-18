import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';

@Component({
  selector: 'bia-button-group',
  templateUrl: './bia-button-group.component.html',
  styleUrl: './bia-button-group.component.scss',
})
export class BiaButtonGroupComponent implements AfterViewInit {
  @ViewChild(Menubar) menubar: Menubar | undefined;
  @Input() buttons: ButtonGroupItem[];
  @Input() alwaysCompact = false;

  private get menubarNativeElement(): HTMLElement | undefined {
    return this.menubar?.el?.nativeElement as HTMLElement;
  }

  ngAfterViewInit(): void {
    if (this.alwaysCompact === true) {
      this.compactButtons();
    }
  }

  private compactButtons() {
    this.menubarNativeElement?.classList.remove('bia-button-group');
    this.menubarNativeElement?.classList.add('bia-button-group-compact');
  }

  onMenubarClick() {
    this.setMenubarsubZIndex();
  }

  setMenubarsubZIndex() {
    const menubarsub = this.menubarNativeElement?.querySelector(
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
