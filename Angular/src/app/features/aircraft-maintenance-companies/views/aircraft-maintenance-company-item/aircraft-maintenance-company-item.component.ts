import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';
import { RouterOutlet } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-aircraft-maintenance-companies-item',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
    ],
    imports: [RouterOutlet, NgIf, BiaSharedModule, AsyncPipe, SpinnerComponent]
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
