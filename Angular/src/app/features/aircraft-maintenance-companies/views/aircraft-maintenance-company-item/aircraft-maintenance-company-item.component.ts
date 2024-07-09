import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-companies-item',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
})
export class AircraftMaintenanceCompanyItemComponent
  extends CrudItemItemComponent<AircraftMaintenanceCompany>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
    protected layoutService: BiaClassicLayoutService
  ) {
    super(injector, aircraftMaintenanceCompanyService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.aircraftMaintenanceCompanyService.crudItem$.subscribe(
        aircraftMaintenanceCompany => {
          // TODO after creation of CRUD Team AircraftMaintenanceCompany : set the field of the item to display in the breadcrump
          if (aircraftMaintenanceCompany?.title) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] =
                aircraftMaintenanceCompany.title;
            });
            this.layoutService.refreshBreadcrumb();
          }
        }
      )
    );
  }
}
