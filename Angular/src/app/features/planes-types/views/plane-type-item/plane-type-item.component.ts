import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import {
  BiaLayoutService,
  CrudItemItemComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { first } from 'rxjs/operators';
import { AppState } from 'src/app/store/state';
import { PlaneType } from '../../model/plane-type';
import { PlaneTypeService } from '../../services/plane-type.service';

@Component({
  selector: 'app-planes-types-item',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
})
export class PlaneTypeItemComponent
  extends CrudItemItemComponent<PlaneType>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public planeTypeService: PlaneTypeService,
    protected layoutService: BiaLayoutService
  ) {
    super(injector, planeTypeService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.planeTypeService.crudItem$.subscribe(planeType => {
        // TODO after creation of CRUD PlaneType : set the field of the item to display in the breadcrump
        if (planeType?.title) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = planeType.title;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );
  }
}
