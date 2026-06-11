import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  BiaLayoutService,
  CrudItemItemComponent,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { Plane } from '../../model/plane';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-planes-specific-item',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
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
