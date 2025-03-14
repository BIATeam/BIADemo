import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';
import { RouterOutlet } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-planes-specific-item',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
    ],
    imports: [RouterOutlet, NgIf, BiaSharedModule, AsyncPipe, SpinnerComponent]
})
export class PlaneItemComponent
  extends CrudItemItemComponent<Plane>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public planeService: PlaneService,
    protected layoutService: BiaLayoutService
  ) {
    super(injector, planeService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.planeService.crudItem$.subscribe(plane => {
        if (plane?.msn) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = plane.msn;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );
  }
}
