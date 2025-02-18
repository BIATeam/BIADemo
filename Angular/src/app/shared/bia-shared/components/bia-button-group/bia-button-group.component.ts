import {
  AfterViewInit,
  Component,
  Input,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';

@Component({
  selector: 'bia-button-group',
  templateUrl: './bia-button-group.component.html',
  styleUrl: './bia-button-group.component.scss',
})
export class BiaButtonGroupComponent implements AfterViewInit, OnDestroy {
  @ViewChild(Menubar) menubar: Menubar;
  @Input() buttons: ButtonGroupItem[];
  @Input() compact = false;
  @Input() autoCompact = true;

  private parentContainerResizeObserver!: ResizeObserver;
  private containerWidth: number;

  private get menubarNativeElement(): HTMLElement {
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

  private initParentContainerResizeObserver() {
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

  private onParentContainerResized(container: Element) {
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

    if (!this.containerWidth) {
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
        container.getBoundingClientRect().right + this.containerWidth + offset;
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

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface ButtonGroupItem extends MenuItem {}
