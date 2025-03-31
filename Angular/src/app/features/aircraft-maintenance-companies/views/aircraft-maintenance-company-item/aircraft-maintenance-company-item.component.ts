import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-companies-item',
  templateUrl:
    '/src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '/src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
})
export class AircraftMaintenanceCompanyItemComponent
  extends CrudItemItemComponent<AircraftMaintenanceCompany>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
    protected layoutService: BiaLayoutService
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
