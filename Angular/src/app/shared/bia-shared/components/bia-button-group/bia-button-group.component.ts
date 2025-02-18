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

  private resizeObserver!: ResizeObserver;
  private containerRect: DOMRect;

  private get menubarNativeElement(): HTMLElement {
    return this.menubar.el.nativeElement as HTMLElement;
  }

  ngAfterViewInit() {
    if (!this.compact) {
      setTimeout(() => {
        this.observeResize();
      }, 1000);
    }
  }

  ngOnDestroy(): void {
    this.resizeObserver.disconnect();
  }

  private observeResize() {
    const container = this.menubarNativeElement.parentElement?.parentElement;
    console.log('Container', container);
    if (!container) {
      return;
    }

    const nextSibling = container.nextElementSibling;
    console.log('Next Sibling', nextSibling);

    const previousSibling = container.previousElementSibling;
    console.log('Previous Sibling', previousSibling);

    if (!nextSibling && !previousSibling) {
      return;
    }

    const parentContainer = container.parentElement;
    if (!parentContainer) {
      return;
    }

    this.resizeObserver = new ResizeObserver(() =>
      this.checkSize(container, nextSibling, previousSibling)
    );

    this.resizeObserver.observe(parentContainer);
  }

  private checkSize(
    container: Element,
    nextSibling: Element | null,
    previousSibling: Element | null
  ) {
    const offset = 20;
    console.log('Check size');

    const containerRect = this.compact
      ? this.containerRect
      : container.getBoundingClientRect();

    if (nextSibling) {
      const mustCompact =
        containerRect.right + offset >=
        nextSibling.getBoundingClientRect().left;
      if (!this.compact && mustCompact) {
        this.containerRect = containerRect;
      }
      this.compact = mustCompact;
      return;
    }

    if (previousSibling) {
      const previousSiblingRect = previousSibling.getBoundingClientRect();
      this.compact =
        previousSiblingRect.right + offset >= this.containerRect.left;
      return;
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
