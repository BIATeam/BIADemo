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
      this.updateStickyPosition();
    });
  }

  @Input() alignFrozen: string = 'left';

  private observer: ResizeObserver | null = null;

  constructor(
    private el: ElementRef,
    private zone: NgZone
  ) {}

  ngAfterViewInit() {
    this.zone.runOutsideAngular(() => {
      setTimeout(() => {
        this.listenToSibling();
        this.recalculateColumns();
      }, 1000);
    });
  }

  ngOnDestroy() {
    this.removeListener();
    this.el.nativeElement.dispatchEvent(new CustomEvent('isDestroyed'));
  }

  recalculateColumns() {
    if (this.el?.nativeElement?.parentNode) {
      const siblings = DomHandler.siblings(this.el.nativeElement);
      const index = DomHandler.index(this.el.nativeElement);
      const time =
        this.alignFrozen === 'left'
          ? (index + 1) * 50
          : (siblings.length - index + 1) * 50;

      setTimeout(() => {
        this.updateStickyPosition();
      }, time);
    }
  }

  _frozen: boolean = true;

  updateStickyPosition() {
    if (this._frozen) {
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
    }
  }

  listenToSibling() {
    this.removeListener();
    if (this._frozen) {
      this.sibling =
        this.alignFrozen === 'left'
          ? this.el.nativeElement.previousElementSibling
          : this.el.nativeElement.nextElementSibling;

      if (this.sibling) {
        this.setupObserver(this.sibling);
        this.sibling.addEventListener('positionChanged', () =>
          this.recalculateColumns()
        );
        this.sibling.addEventListener('isDestroyed', () =>
          this.listenToSibling()
        );
      }
    }
  }

  setupObserver(observedElement: HTMLElement) {
    this.observer = new ResizeObserver(() => {
      this.recalculateColumns();
    });

    this.observer.observe(observedElement);
  }

  removeListener() {
    if (this.sibling) {
      this.sibling.removeEventListener('positionChanged', () =>
        this.recalculateColumns()
      );
      this.sibling.removeEventListener('isDestroyed', () =>
        this.listenToSibling()
      );
    }
  }
}
