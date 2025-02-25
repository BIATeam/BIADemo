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
import { ActivatedRoute, Router } from '@angular/router';
import { BiaInjectorService } from 'src/app/core/bia-core/services/bia-injector.service';
import { LayoutHelperService } from '../../../services/layout-helper.service';
import { BiaLayoutService } from '../services/layout.service';

@Component({
  selector: 'bia-split-page-layout',
  templateUrl: './split-page-layout.component.html',
  styleUrls: ['./split-page-layout.component.scss'],
})
export class SplitPageLayoutComponent
  implements OnInit, OnDestroy, AfterViewInit
{
  @ViewChild('dynamic', {
    read: ViewContainerRef,
  })
  viewContainerRef: ViewContainerRef;
  @ViewChild('leftContainer') leftContainer: ElementRef;
  @ViewChild('rightContainer') rightContainer: ElementRef;

  private isResizing = false;
  private startX: number;
  private startWidthLeft: number;
  private startWidthRight: number;

  leftWidth = 70;
  heightOffset = '+ 1.5rem';

  constructor(
    public activatedRoute: ActivatedRoute,
    protected router: Router,
    protected serviceInjector: BiaInjectorService,
    protected readonly layoutService: BiaLayoutService,
    private renderer: Renderer2
  ) {}

  get defaultContainerHeight(): string {
    return `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService, this.heightOffset)}`;
  }

  pageTitle: string;
  protected dynamicComponent: ComponentRef<any>;
  @HostBinding('class.flex') flex = true;

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.pageTitle = snapshot.data['title'];
    this.leftWidth = snapshot.data['leftWidth'] ?? 70;
    this.heightOffset = snapshot.data['heightOffset'] ?? '+ 1.5rem';
  }

  ngAfterViewInit() {
    setTimeout(() => {
      const snapshot = this.activatedRoute.snapshot;
      this.dynamicComponent = this.serviceInjector.addDynamicComponent(
        this.viewContainerRef,
        snapshot.data['injectComponent']
      );
    }, 0);
    // this.renderer.setStyle(this.leftContainer.nativeElement, 'flex', '1');
    // this.renderer.setStyle(this.rightContainer.nativeElement, 'flex', '1');
  }

  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
  }

  startResize(event: MouseEvent) {
    this.isResizing = true;
    this.startX = event.clientX;
    this.startWidthLeft = this.leftContainer.nativeElement.offsetWidth;
    this.startWidthRight = this.rightContainer.nativeElement.offsetWidth;
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    if (this.isResizing) {
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
