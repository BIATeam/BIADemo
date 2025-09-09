import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'packages/bia-ng/shared/public-api';
import { of } from 'rxjs';
import { FlightFormComponent } from '../../components/flight-form/flight-form.component';
import { flightCRUDConfiguration } from '../../flight.constants';
import { Flight } from '../../model/flight';
import { FlightOptionsService } from '../../services/flight-options.service';
import { FlightService } from '../../services/flight.service';

@Component({
  selector: 'app-flight-new',
  templateUrl: './flight-new.component.html',
  imports: [FlightFormComponent, AsyncPipe],
})
export class FlightNewComponent
  extends CrudItemNewComponent<Flight>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    protected flightOptionsService: FlightOptionsService,
    public flightService: FlightService
  ) {
    super(injector, flightService);
    if (!this.itemTemplate$) {
      this.itemTemplate$ = of(<Flight>{ id: '' });
    }
    this.crudConfiguration = flightCRUDConfiguration;
  }
}
