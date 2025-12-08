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
    // eslint-disable-next-line @typescript-eslint/naming-convention
    '[class.p-datatable-frozen-column]': 'frozen',
    // eslint-disable-next-line @typescript-eslint/naming-convention
    '[class.p-datatable-frozen-column-left]':
      'frozen && alignFrozen === "left"',
    // eslint-disable-next-line @typescript-eslint/naming-convention
    '[class.p-datatable-frozen-column-right]':
      'frozen && alignFrozen === "right"',
  },
})
export class BiaFrozenColumnDirective implements AfterViewInit, OnDestroy {
  _frozen = true;

  @Input() get frozen(): boolean {
    return this._frozen;
  }

  set frozen(val: boolean) {
    this._frozen = val;
    if (val) {
      Promise.resolve(null).then(() => {
        this.listenToOtherTableElements();
        this.updateStickyPosition(true);
      });
    } else {
      this.removeListeners();
    }
  }

  @Input() alignFrozen: string = 'left';

  sibling: any;
  firstFrozenCellInColumn: any;
  isfirstFrozenCellInColumn = false;
  siblingResizeObserver: ResizeObserver | null = null;
  rowMutationObserver: MutationObserver;
  isFirstFrozenCellInColumnDestroyedHandler: () => void = () =>
    this.initSiblings();
  positionChangedHandler: () => void = () =>
    this.updateStickyPositionWithDelay(true);
  isSiblingDestroyedHandler: () => void = () =>
    this.listenToOtherTableElements();

  constructor(
    private el: ElementRef,
    private zone: NgZone
  ) {}

  ngAfterViewInit() {
    this.zone.runOutsideAngular(() => {
      setTimeout(() => {
        this.initSiblings();
        this.updateStickyPositionWithDelay(false);
      }, 200);
    });
  }

  initSiblings() {
    this.setFirstFrozenColumnCell();
    if (this.isfirstFrozenCellInColumn) {
      this.listenToOtherTableElements();
    }
  }

  ngOnDestroy() {
    this.removeListeners();
    this.el.nativeElement.dispatchEvent(new CustomEvent('isDestroyed'));
  }

  setFirstFrozenColumnCell() {
    if (this.el?.nativeElement?.parentNode) {
      this.firstFrozenCellInColumn = null;
      const index = DomHandler.index(this.el.nativeElement);
      // get the thead, tbody, tfooter, etc. node
      const childNodes =
        this.el.nativeElement.parentNode?.parentNode?.childNodes;

      // Check the row until first frozen cell at same index in row as current element is found
      if (childNodes) {
        for (let i = 0; i < childNodes.length; i++) {
          if (childNodes[i].children) {
            const children = childNodes[i].children[index];
            if (children && children.hasAttribute('biaFrozenColumn')) {
              if (children !== this.el.nativeElement) {
                this.firstFrozenCellInColumn = childNodes[i].children[index];
                this.isfirstFrozenCellInColumn = false;
              } else {
                this.isfirstFrozenCellInColumn = true;
                this.firstFrozenCellInColumn = null;
              }
              break;
            }
          }
        }
      }
    }
  }

  updateStickyPositionWithDelay(cascading: boolean) {
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

  updateStickyPosition(cascading: boolean) {
    if ((this.isfirstFrozenCellInColumn || !cascading) && this._frozen) {
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

      // Only first frozen cell is setting all the others cells position when cascading is on
      if (cascading) {
        const index = DomHandler.index(this.el.nativeElement);
        let nextRow = this.el.nativeElement?.parentElement?.nextElementSibling;
        while (nextRow) {
          if (
            nextRow.children &&
            nextRow.children[index] &&
            nextRow.children[index].hasAttribute('biaFrozenColumn')
          ) {
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

  listenToOtherTableElements() {
    this.removeListeners();
    if (this._frozen) {
      this.firstFrozenCellInColumn?.addEventListener(
        'isDestroyed',
        this.isFirstFrozenCellInColumnDestroyedHandler
      );
      if (this.isfirstFrozenCellInColumn) {
        this.sibling =
          this.alignFrozen === 'left'
            ? this.el.nativeElement.previousElementSibling
            : this.el.nativeElement.nextElementSibling;

        if (this.sibling) {
          this.setupSiblingResizeObserver(this.sibling);
          this.sibling.addEventListener(
            'positionChanged',
            this.positionChangedHandler
          );
          this.sibling.addEventListener(
            'isDestroyed',
            this.isSiblingDestroyedHandler
          );
        }
        this.setupRowMutationObserver(this.el.nativeElement.parentNode);
      }
    }
  }

  setupSiblingResizeObserver(observedElement: HTMLElement) {
    // Observe the right (or left) element to adapt position if it changed width
    if (observedElement) {
      this.siblingResizeObserver = new ResizeObserver(() => {
        this.updateStickyPositionWithDelay(true);
      });
      this.siblingResizeObserver.observe(observedElement);
    }
  }

  setupRowMutationObserver(observedElement: HTMLElement) {
    // Observe if a column is moved, added or removed in the row of the Frozen cell
    if (observedElement) {
      this.rowMutationObserver = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
          if (mutation.type === 'childList') {
            this.initSiblings();
          }
        });
      });
      this.rowMutationObserver.observe(observedElement, {
        childList: true,
        subtree: false,
      });
    }
  }

  removeListeners() {
    this.firstFrozenCellInColumn?.removeEventListener(
      'isDestroyed',
      this.isFirstFrozenCellInColumnDestroyedHandler
    );
    if (this.sibling) {
      this.siblingResizeObserver?.unobserve(this.sibling);
      this.sibling.removeEventListener(
        'positionChanged',
        this.positionChangedHandler
      );
      this.sibling.removeEventListener(
        'isDestroyed',
        this.isSiblingDestroyedHandler
      );
    }
    this.rowMutationObserver?.disconnect();
  }
}
