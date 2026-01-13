import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  CrudItemEditComponent,
  FormReadOnlyMode,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { filter } from 'rxjs';
import { Permission } from 'src/app/shared/permission';
import { FlightFormComponent } from '../../components/flight-form/flight-form.component';
import { flightCRUDConfiguration } from '../../flight.constants';
import { Flight } from '../../model/flight';
import { FlightOptionsService } from '../../services/flight-options.service';
import { FlightService } from '../../services/flight.service';

@Component({
  selector: 'app-flight-edit',
  templateUrl: './flight-edit.component.html',
  imports: [FlightFormComponent, AsyncPipe, SpinnerComponent],
})
export class FlightEditComponent
  extends CrudItemEditComponent<Flight>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    protected flightOptionsService: FlightOptionsService,
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
          this.formReadOnlyMode =
            this.crudConfiguration.isFixable === true && flight.isFixed === true
              ? FormReadOnlyMode.on
              : FormReadOnlyMode.off;
        })
    );
  }
}
