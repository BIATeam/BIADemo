import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemReadComponent,
  FormReadOnlyMode,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { filter } from 'rxjs';
import { Permission } from 'src/app/shared/permission';
import { FlightFormComponent } from '../../components/flight-form/flight-form.component';
import { flightCRUDConfiguration } from '../../flight.constants';
import { Flight } from '../../model/flight';
import { FlightService } from '../../services/flight.service';

@Component({
  selector: 'app-flight-read',
  templateUrl: './flight-read.component.html',
  imports: [FlightFormComponent, AsyncPipe, SpinnerComponent],
})
export class FlightReadComponent extends CrudItemReadComponent<Flight> {
  constructor(
    protected injector: Injector,
    public flightService: FlightService
  ) {
    super(injector, flightService);
    this.crudConfiguration = flightCRUDConfiguration;
  }

  protected setPermissions(): void {
    super.setPermissions();
    this.canFix = this.authService.hasPermission(Permission.Flight_Fix);
    this.permissionSub.add(
      this.crudItemService.crudItem$
        .pipe(filter(flight => !!flight && Object.keys(flight).length > 0))
        .subscribe(flight => {
          this.canEdit =
            this.crudConfiguration.isFixable === true && flight.isFixed === true
              ? false
              : this.authService.hasPermission(Permission.Flight_Update);

          this.formReadOnlyMode =
            this.canEdit === false &&
            this.crudConfiguration.isFixable === true &&
            flight.isFixed === true
              ? FormReadOnlyMode.on
              : this.initialFormReadOnlyMode;
        })
    );
  }
}
