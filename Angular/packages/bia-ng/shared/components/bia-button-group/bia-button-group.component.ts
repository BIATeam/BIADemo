import { NgClass, NgIf } from '@angular/common';
import {
  AfterViewInit,
  Component,
  Input,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { MenuItem, MenuItemCommandEvent } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { Menubar } from 'primeng/menubar';
import { Tooltip } from 'primeng/tooltip';

@Component({
  selector: 'bia-button-group',
  templateUrl: './bia-button-group.component.html',
  imports: [Menubar, NgClass, NgIf, ButtonDirective, Tooltip],
})
export class BiaButtonGroupComponent implements AfterViewInit, OnDestroy {
  @ViewChild(Menubar) menubar: Menubar;
  @Input() buttons: BiaButtonGroupItem[];
  @Input() compact = false;
  @Input() autoCompact = true;

  protected parentContainerResizeObserver!: ResizeObserver;
  protected containerWidth: number;

  protected get menubarNativeElement(): HTMLElement {
    return this.menubar.el.nativeElement as HTMLElement;
  }

  ngAfterViewInit() {
    if (this.autoCompact && !this.compact) {
      setTimeout(() => {
        this.initParentContainerResizeObserver();
      }, 500);
    }
  }

  ngOnDestroy(): void {
    if (this.parentContainerResizeObserver) {
      this.parentContainerResizeObserver.disconnect();
    }
  }

  protected initParentContainerResizeObserver() {
    // Container -> BiaButtonGroup -> Menubar
    const container = this.menubarNativeElement.parentElement?.parentElement;
    if (!container) {
      return;
    }

    const parentContainer = container.parentElement;
    if (!parentContainer || parentContainer.children.length === 1) {
      return;
    }

    this.parentContainerResizeObserver = new ResizeObserver(() =>
      this.onParentContainerResized(container)
    );
    this.parentContainerResizeObserver.observe(parentContainer);
  }

  protected onParentContainerResized(container: Element) {
    const offset = 10;

    // Handle case when burger button is displayed by native primeng menubar
    if (!this.compact) {
      const burgerButton =
        this.menubarNativeElement.querySelector('.p-menubar-button');
      if (burgerButton) {
        const burgerButtonStyle = window.getComputedStyle(burgerButton);
        if (burgerButtonStyle.display === 'flex') {
          return;
        }
      }
    }

    if (!this.containerWidth || !this.compact) {
      this.containerWidth = container.getBoundingClientRect().width;
    }

    if (container.nextElementSibling) {
      this.compact =
        container.getBoundingClientRect().left + this.containerWidth + offset >=
        container.nextElementSibling.getBoundingClientRect().left;
      return;
    }

    if (container.previousElementSibling) {
      this.compact =
        container.previousElementSibling.getBoundingClientRect().right >=
        container.getBoundingClientRect().right - this.containerWidth - offset;
    }
  }

  onMenubarClick() {
    this.setMenubarsubZIndex();
  }

  setMenubarsubZIndex() {
    const menubarsub = this.menubarNativeElement.querySelector(
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

export class BiaButtonGroupItem implements MenuItem {
  label?: string | undefined;
  tooltipPosition?: string | undefined;
  tooltip?: string | undefined;
  visible?: boolean | undefined;
  disabled?: boolean | undefined;
  // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-empty-function
  command(event: MenuItemCommandEvent): void {}

  constructor(
    label: string,
    command: (event: MenuItemCommandEvent) => void,
    visible?: boolean | undefined,
    disabled?: boolean | undefined,
    tooltip?: string | undefined,
    tooltipPosition?: string | undefined
  ) {
    this.label = label;
    this.command = command;
    this.visible = visible ?? true;
    this.disabled = disabled ?? false;
    this.tooltip = tooltip;
    if (this.tooltip) {
      this.tooltipPosition = tooltipPosition ?? 'top';
    }
  }
}
