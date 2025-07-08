import { NgClass, NgIf, NgStyle } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ComponentRef,
  ElementRef,
  HostBinding,
  HostListener,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  NavigationEnd,
  Router,
  RouterOutlet,
} from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { BehaviorSubject, filter } from 'rxjs';
import { BiaInjectorService } from 'src/app/core/bia-core/services/bia-injector.service';
import { CrudConfig } from '../../../feature-templates/crud-items/model/crud-config';
import { LayoutHelperService } from '../../../services/layout-helper.service';
import { BiaLayoutService } from '../services/layout.service';

export enum LayoutMode {
  popup,
  fullPage,
  splitPage,
}

@Component({
  selector: 'bia-dynamic-layout',
  templateUrl: './dynamic-layout.component.html',
  styleUrls: ['./dynamic-layout.component.scss'],
  imports: [
    NgIf,
    NgClass,
    NgStyle,
    RouterOutlet,
    Dialog,
    SharedModule,
    TranslateModule,
  ],
})
export class DynamicLayoutComponent<TDto extends { id: number }>
  implements OnInit, AfterViewInit, OnDestroy
{
  @ViewChild('dynamic', {
    read: ViewContainerRef,
  })
  viewContainerRef: ViewContainerRef;
  @ViewChild('leftContainer') leftContainer: ElementRef;
  @ViewChild('rightContainer') rightContainer: ElementRef;

  configuration?: CrudConfig<TDto>;
  hasChildren = false;
  layoutMode?: LayoutMode;
  forceSplitPage = false;
  forcePopup = false;
  pageTitle: string;
  $displayPageComponent: BehaviorSubject<boolean>;
  displayPageComponent: boolean;
  dynamicComponent: ComponentRef<any>;

  leftWidth = 70;
  heightOffset = '+ 1.5rem';
  allowSplitScreenResize = true;

  popupTitle: string;
  style: any;
  maximizable: boolean;

  protected maxScanDepth = 2;
  protected isResizing = false;
  protected startX: number;
  protected startWidthLeft: number;
  protected startWidthRight: number;

  constructor(
    public activatedRoute: ActivatedRoute,
    protected router: Router,
    protected serviceInjector: BiaInjectorService,
    protected readonly renderer: Renderer2,
    protected readonly layoutService: BiaLayoutService
  ) {}

  @HostBinding('class.bia-split-page-layout') get isSplit(): boolean {
    return (
      (this.configuration?.useSplit && this.layoutMode === undefined) ||
      this.layoutMode === LayoutMode.splitPage
    );
  }

  previousIsSplit = false;
  get isSplitWithVisibilityCheck(): boolean {
    const isSplit =
      (this.configuration?.useSplit && this.layoutMode === undefined) ||
      this.layoutMode === LayoutMode.splitPage;
    if (this.previousIsSplit !== isSplit) {
      this.checkChildrenRules();
    }
    this.previousIsSplit = isSplit;
    return isSplit;
  }

  get isPopup(): boolean {
    return (
      (this.configuration?.usePopup && this.layoutMode === undefined) ||
      this.layoutMode === LayoutMode.popup
    );
  }

  previousIsPopup = false;
  get isPopupWithVisibilityCheck(): boolean {
    const isPopup =
      (this.configuration?.usePopup && this.layoutMode === undefined) ||
      this.layoutMode === LayoutMode.popup;
    if (this.previousIsPopup !== isPopup) {
      this.checkChildrenRules();
    }
    this.previousIsPopup = isPopup;
    return isPopup;
  }

  get defaultContainerHeight(): string {
    return `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService, this.heightOffset)}`;
  }

  get shouldDisplayPageComponent(): boolean {
    return !this.hasChildren || this.isSplit || this.isPopup;
  }

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.pageTitle = snapshot.data['title'];
    this.$displayPageComponent = new BehaviorSubject<boolean>(true);
    this.displayPageComponent = true;
    this.configuration = snapshot.data['configuration'];
    this.leftWidth = snapshot.data['leftWidth'] ?? this.leftWidth;
    this.heightOffset = snapshot.data['heightOffset'] ?? this.heightOffset;
    this.previousIsSplit = this.configuration?.useSplit ?? false;
    this.maxScanDepth = snapshot.data['maxScanDepth'] ?? this.maxScanDepth;
    this.allowSplitScreenResize =
      snapshot.data['allowSplitScreenResize'] ?? true;
    this.checkChildrenRules();
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.checkChildrenRules();
      });
  }

  checkChildrenRules() {
    const snapshot = this.activatedRoute.snapshot;
    let child: ActivatedRouteSnapshot | null;
    child = snapshot.firstChild;
    let childScanDepth = 0;
    this.hasChildren = false;
    this.layoutMode = undefined;
    this.style = { minWidth: '50vw' };
    this.popupTitle = '';
    while (child && childScanDepth < this.maxScanDepth) {
      this.hasChildren = true;
      this.popupTitle = child.data['title'] ?? this.popupTitle;
      this.style = child.data['style'] ?? this.style;
      this.maximizable = child.data['maximizable'] ?? true;

      if (child.data['layoutMode'] !== undefined) {
        this.layoutMode = child.data['layoutMode'];
      }
      child = child?.firstChild;
      childScanDepth++;
    }

    if (this.shouldDisplayPageComponent !== this.$displayPageComponent.value) {
      this.$displayPageComponent.next(this.shouldDisplayPageComponent);
      this.displayPageComponent = this.shouldDisplayPageComponent;
    }
  }

  ngAfterViewInit() {
    this.$displayPageComponent.subscribe(() => {
      const snapshot = this.activatedRoute.snapshot;
      if (
        this.$displayPageComponent.value &&
        this.dynamicComponent === undefined
      ) {
        setTimeout(() => {
          this.dynamicComponent = this.serviceInjector.addDynamicComponent(
            this.viewContainerRef,
            snapshot.data['injectComponent']
          );
        }, 0);
      } else {
        if (this.dynamicComponent !== undefined) {
          if (this.$displayPageComponent.value) {
            this.dynamicComponent.instance.onDisplay();
          } else {
            this.dynamicComponent.instance.onHide();
          }
        }
      }
    });
  }

  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
  }

  startResize(event: MouseEvent) {
    if (this.allowSplitScreenResize) {
      this.isResizing = true;
      this.startX = event.clientX;
      this.startWidthLeft = this.leftContainer.nativeElement.offsetWidth;
      this.startWidthRight = this.rightContainer.nativeElement.offsetWidth;
    }
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    if (this.allowSplitScreenResize && this.isResizing) {
      const deltaX = event.clientX - this.startX;
      const newWidthLeft = this.startWidthLeft + deltaX;
      const newWidthRight = this.startWidthRight - deltaX;

      this.renderer.setStyle(
        this.leftContainer.nativeElement,
        'flex',
        `${newWidthLeft}px`
      );
      this.renderer.setStyle(
        this.rightContainer.nativeElement,
        'flex',
        `${newWidthRight}px`
      );
    }
  }

  @HostListener('document:mouseup')
  onMouseUp() {
    this.isResizing = false;
  }
}
