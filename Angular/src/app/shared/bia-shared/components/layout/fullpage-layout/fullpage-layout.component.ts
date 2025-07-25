import { NgIf } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ComponentRef,
  HostBinding,
  OnDestroy,
  OnInit,
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
import { BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { BiaInjectorService } from 'src/app/core/bia-core/services/bia-injector.service';

@Component({
  selector: 'bia-full-page-layout',
  templateUrl: './fullpage-layout.component.html',
  styleUrls: ['./fullpage-layout.component.scss'],
  imports: [NgIf, RouterOutlet, TranslateModule],
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
      .pipe(filter(event => event instanceof NavigationEnd))
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
        // if( this.dynamicComponent !== undefined ) this.dynamicComponent.destroy();
      }
    });
  }
  ngOnDestroy() {
    if (this.dynamicComponent !== undefined) {
      this.dynamicComponent.destroy();
    }
  }
}
