import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Plane } from '../../model/plane';
import { ActivatedRoute } from '@angular/router';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { PlaneService } from '../../services/plane.service';

@Component({
  templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: ['../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss']
})
export class PlaneItemComponent extends CrudItemItemComponent<Plane> {
  constructor(protected store: Store<AppState>,
    protected route: ActivatedRoute,
    protected layoutService: BiaClassicLayoutService,
    public planeService: PlaneService,
  ) {
    super(route,planeService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add
      (
        this.planeService.crudItem$.subscribe((plane) => {
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
