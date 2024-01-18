import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { PlaneType } from '../../model/plane-type';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { PlaneTypeService } from '../../services/plane-type.service';

@Component({
  selector: 'app-planes-types-item',
  templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: ['../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss']
})
export class PlaneTypeItemComponent extends CrudItemItemComponent<PlaneType> implements OnInit {
  constructor(protected store: Store<AppState>,
    protected injector: Injector,
    public planeTypeService: PlaneTypeService,
    protected layoutService: BiaClassicLayoutService,
  ) {
    super(injector, planeTypeService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add
      (
        this.planeTypeService.crudItem$.subscribe((planeType) => {
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
