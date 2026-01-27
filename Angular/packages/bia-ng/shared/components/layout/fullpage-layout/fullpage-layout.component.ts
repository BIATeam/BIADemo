import {
  AfterViewInit,
  Component,
  ComponentRef,
  DestroyRef,
  HostBinding,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewContainerRef,
  inject,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  NavigationEnd,
  Router,
  RouterOutlet,
} from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { BiaInjectorService } from 'packages/bia-ng/core/public-api';
import { BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';

/**
 * @deprecated This class is deprecated. Use DynamicLayoutComponent instead. See documentation in BiaDocs
 */
@Component({
  selector: 'bia-full-page-layout',
  templateUrl: './fullpage-layout.component.html',
  styleUrls: ['./fullpage-layout.component.scss'],
  imports: [RouterOutlet, TranslateModule],
})
export class FullPageLayoutComponent
  implements OnInit, OnDestroy, AfterViewInit
{
  @ViewChild('dynamic', {
    read: ViewContainerRef,
  })
  viewContainerRef: ViewContainerRef;

  constructor(
    public activatedRoute: ActivatedRoute,
    protected router: Router,
    protected serviceInjector: BiaInjectorService
  ) {}

  private readonly destroyRef = inject(DestroyRef);

  pageTitle: string;
  public $displayPageComponent: BehaviorSubject<boolean>;
  public displayPageComponent: boolean;
  protected dynamicComponent: ComponentRef<any>;
  @HostBinding('class.bia-flex') flex = false;

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.pageTitle = snapshot.data['title'];
    this.$displayPageComponent = new BehaviorSubject<boolean>(true);
    this.displayPageComponent = true;
    this.checkVisibility();
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => {
        this.checkVisibility();
      });
  }

  checkVisibility() {
    const snapshot = this.activatedRoute.snapshot;
    let child: ActivatedRouteSnapshot | null;
    child = snapshot.firstChild;
    let hasFullPageChild = false;
    while (child) {
      // eslint-disable-next-line @typescript-eslint/no-deprecated
      if (child.component === FullPageLayoutComponent) {
        hasFullPageChild = true;
        break;
      }
      child = child?.firstChild;
    }
    if (hasFullPageChild === this.$displayPageComponent.value) {
      this.$displayPageComponent.next(!hasFullPageChild);
      this.displayPageComponent = !hasFullPageChild;
    }
  }
  ngAfterViewInit() {
    this.$displayPageComponent
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
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
        // if( this.dynamicComponent !== undefined ) this.dynamicComponent.destroy();
      });
  }
  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
  }
}
