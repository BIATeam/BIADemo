import {
  AfterViewInit,
  Component,
  ComponentRef,
  HostBinding,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Dialog } from 'primeng/dialog';
import { Subscription } from 'rxjs';
import { BiaInjectorService } from 'src/app/core/bia-core/services/bia-injector.service';

@Component({
  selector: 'bia-popup-layout',
  templateUrl: './popup-layout.component.html',
})
export class PopupLayoutComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('dynamic', {
    read: ViewContainerRef,
  })
  viewContainerRef: ViewContainerRef;
  @ViewChild('dialog', { static: false }) dialog!: Dialog;
  subs: Subscription = new Subscription();

  constructor(
    public activatedRoute: ActivatedRoute,
    private renderer: Renderer2,
    protected serviceInjector: BiaInjectorService
  ) {}
  protected dynamicComponent: ComponentRef<any>;

  popupTitle: string;
  style: any;
  maximizable: boolean;
  @HostBinding('class') classes = 'bia-flex';

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;

    this.popupTitle = snapshot.data['title'];
    this.style = { minWidth: '50vw' };
    if (snapshot.data['style']) {
      this.style = snapshot.data['style'];
    }

    this.maximizable = snapshot.data['maximizable'] ?? true;
  }
  ngAfterViewInit() {
    setTimeout(() => {
      this.dynamicComponent = this.serviceInjector.addDynamicComponent(
        this.viewContainerRef,
        this.activatedRoute.snapshot.data['injectComponent']
      );
      this.setFocusOnFirstField();
    }, 0);

    const firstField =
      this.viewContainerRef.element.nativeElement.parentNode.querySelector(
        'input, select, textarea'
      );
    if (firstField) {
      firstField.focus();
    }

    this.subs.add(
      this.dialog.onShow.subscribe(() => {
        const maximizeButton = document.querySelector(
          '.p-dialog-header-maximize'
        );
        if (maximizeButton) {
          this.renderer.listen(maximizeButton, 'focus', () => {
            console.log('Focus');
            (maximizeButton as HTMLElement).blur();
          });
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
    this.subs.unsubscribe();
  }

  private setFocusOnFirstField() {
    // Attendre que le contenu soit bien rendu dans le DOM
    const firstField =
      this.viewContainerRef.element.nativeElement.parentNode.querySelector(
        'input, select, textarea'
      );
    if (firstField) {
      firstField.focus();
    }
  }
}
