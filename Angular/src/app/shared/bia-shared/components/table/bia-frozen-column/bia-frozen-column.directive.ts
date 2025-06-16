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
  firstCellMaster: any;
  _frozen: boolean = true;

  @Input() get frozen(): boolean {
    return this._frozen;
  }

  set frozen(val: boolean) {
    this._frozen = val;
    if (val) {
      Promise.resolve(null).then(() => {
        this.listenToSibling();
        this.updateStickyPosition(true);
      });
    } else {
      this.removeListeners();
    }
  }

  @Input() alignFrozen: string = 'left';

  isFirstRow = false;
  resizeObserver: ResizeObserver | null = null;
  mutationObserver: MutationObserver;

  constructor(
    private el: ElementRef,
    private zone: NgZone
  ) {}

  ngAfterViewInit() {
    this.zone.runOutsideAngular(() => {
      setTimeout(() => {
        this.initSiblings();
        this.recalculateColumns(false);
      }, 200);
    });
  }

  initSiblings() {
    this.getFrozenMaster();
    if (this.isFirstRow) {
      this.listenToSibling();
    }
  }

  ngOnDestroy() {
    this.removeListeners();
    this.el.nativeElement.dispatchEvent(new CustomEvent('isDestroyed'));
  }

  getFrozenMaster() {
    if (this.el?.nativeElement?.parentNode) {
      this.firstCellMaster = null;
      const index = DomHandler.index(this.el.nativeElement);
      const childNodes =
        this.el.nativeElement.parentNode?.parentNode?.childNodes;

      if (childNodes) {
        for (var i = 0; i < childNodes.length; i++) {
          if (childNodes[i].children) {
            const children = childNodes[i].children[index];
            if (children && children.hasAttribute('biaFrozenColumn')) {
              if (children != this.el.nativeElement) {
                this.firstCellMaster = childNodes[i].children[index];
              } else {
                this.isFirstRow =
                  this.el.nativeElement?.parentElement
                    ?.previousElementSibling == null;
              }
              break;
            }
          }
        }
      }
    }
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
    this.removeListeners();
    if (this._frozen) {
      this.firstCellMaster?.addEventListener('masterDestroyed', () =>
        this.initSiblings()
      );
      if (this.isFirstRow) {
        this.sibling =
          this.alignFrozen === 'left'
            ? this.el.nativeElement.previousElementSibling
            : this.el.nativeElement.nextElementSibling;

        if (this.sibling) {
          this.setupResizeObserver(this.sibling);
          this.sibling.addEventListener('positionChanged', () =>
            this.recalculateColumns(true)
          );
          this.sibling.addEventListener('isDestroyed', () =>
            this.listenToSibling()
          );
        }
        this.setupMutationObserver(this.el.nativeElement.parentNode);
      }
    }
  }

  setupResizeObserver(observedElement: HTMLElement) {
    if (observedElement) {
      this.resizeObserver = new ResizeObserver(() => {
        this.recalculateColumns(true);
      });
      this.resizeObserver.observe(observedElement);
    }
  }

  setupMutationObserver(observedElement: HTMLElement) {
    if (observedElement) {
      this.mutationObserver = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
          if (mutation.type === 'childList') {
            this.initSiblings();
          }
        });
      });
      this.mutationObserver.observe(observedElement, {
        childList: true,
        subtree: false,
      });
    }
  }

  removeListeners() {
    this.firstCellMaster?.removeEventListener('masterDestroyed', () =>
      this.initSiblings()
    );
    if (this.sibling) {
      this.resizeObserver?.unobserve(this.sibling);
      this.sibling.removeEventListener('positionChanged', () =>
        this.recalculateColumns(true)
      );
      this.sibling.removeEventListener('isDestroyed', () =>
        this.listenToSibling()
      );
    }
    this.mutationObserver?.disconnect();
  }
}
