import {
  AfterViewInit,
  Directive,
  ElementRef,
  Input,
  NgZone,
  OnDestroy,
} from '@angular/core';
import { DomHandler } from 'primeng/dom';

@Directive({
  selector: '[biaFrozenColumn]',
  standalone: true,
  host: {
    '[class.p-datatable-frozen-column]': 'frozen',
    '[class.p-datatable-frozen-column-left]': 'alignFrozen === "left"',
  },
})
export class BiaFrozenColumn implements AfterViewInit, OnDestroy {
  sibling: any;
  @Input() get frozen(): boolean {
    return this._frozen;
  }

  set frozen(val: boolean) {
    this._frozen = val;
    Promise.resolve(null).then(() => {
      this.listenToSibling();
      this.updateStickyPosition(true);
    });
  }

  @Input() alignFrozen: string = 'left';

  isFirstRow = false;
  private observer: ResizeObserver | null = null;

  constructor(
    private el: ElementRef,
    private zone: NgZone
  ) {}

  ngAfterViewInit() {
    this.zone.runOutsideAngular(() => {
      setTimeout(() => {
        this.isFirstRow =
          this.el.nativeElement?.parentElement?.previousElementSibling == null;
        if (this.isFirstRow) {
          this.listenToSibling();
        }
        this.recalculateColumns(false);
      }, 1000);
    });
  }

  ngOnDestroy() {
    this.removeListener();
    this.el.nativeElement.dispatchEvent(new CustomEvent('isDestroyed'));
  }

  recalculateColumns(cascading: boolean) {
    if (this.el?.nativeElement?.parentNode) {
      const siblings = DomHandler.siblings(this.el.nativeElement);
      const index = DomHandler.index(this.el.nativeElement);
      const time =
        this.alignFrozen === 'left'
          ? (index + 1) * 50
          : (siblings.length - index + 1) * 50;

      setTimeout(() => {
        this.updateStickyPosition(cascading);
      }, time);
    }
  }

  _frozen: boolean = true;

  updateStickyPosition(cascading: boolean) {
    if ((this.isFirstRow || !cascading) && this._frozen) {
      if (this.alignFrozen === 'right') {
        let right = 0;
        let sibling = this.el.nativeElement.nextElementSibling;
        while (sibling) {
          right += DomHandler.getOuterWidth(sibling);
          sibling = sibling.nextElementSibling;
        }
        this.el.nativeElement.style.right = right + 'px';
        this.el.nativeElement.dispatchEvent(new CustomEvent('positionChanged'));
      } else {
        let left = 0;
        let sibling = this.el.nativeElement.previousElementSibling;
        while (sibling) {
          left += DomHandler.getOuterWidth(sibling);
          sibling = sibling.previousElementSibling;
        }
        this.el.nativeElement.style.left = left + 'px';
        this.el.nativeElement.dispatchEvent(new CustomEvent('positionChanged'));
      }

      if (cascading) {
        let nextRow = this.el.nativeElement?.parentElement?.nextElementSibling;
        while (nextRow) {
          let index = DomHandler.index(this.el.nativeElement);
          if (nextRow.children && nextRow.children[index]) {
            nextRow.children[index].style.left =
              this.el.nativeElement.style.left;
            nextRow.children[index].style.right =
              this.el.nativeElement.style.right;
          }

          nextRow = nextRow.nextElementSibling;
        }
      }
    }
  }

  listenToSibling() {
    this.removeListener();
    if (this.isFirstRow && this._frozen) {
      this.sibling =
        this.alignFrozen === 'left'
          ? this.el.nativeElement.previousElementSibling
          : this.el.nativeElement.nextElementSibling;

      if (this.sibling) {
        this.setupObserver(this.sibling);
        this.sibling.addEventListener('positionChanged', () =>
          this.recalculateColumns(true)
        );
        this.sibling.addEventListener('isDestroyed', () =>
          this.listenToSibling()
        );
      }
    }
  }

  setupObserver(observedElement: HTMLElement) {
    this.observer = new ResizeObserver(() => {
      this.recalculateColumns(true);
    });

    this.observer.observe(observedElement);
  }

  removeListener() {
    if (this.sibling) {
      this.observer?.unobserve(this.sibling);
      this.sibling.removeEventListener('positionChanged', () =>
        this.recalculateColumns(true)
      );
      this.sibling.removeEventListener('isDestroyed', () =>
        this.listenToSibling()
      );
    }
  }
}
