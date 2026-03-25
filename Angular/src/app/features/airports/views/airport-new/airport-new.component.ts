import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from '@bia-team/bia-ng/shared';
import { airportCRUDConfiguration } from '../../airport.constants';
import { AirportFormComponent } from '../../components/airport-form/airport-form.component';
import { Airport } from '../../model/airport';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-airport-new',
  templateUrl: './airport-new.component.html',
  imports: [AirportFormComponent, AsyncPipe],
})
export class AirportNewComponent extends CrudItemNewComponent<Airport> {
  constructor(
    protected injector: Injector,
    public airportService: AirportService
  ) {
    super(injector, airportService);
    this.crudConfiguration = airportCRUDConfiguration;
  }
}
