import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { airportCRUDConfiguration } from '../../airport.constants';
import { Airport } from '../../model/airport';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-airport-new',
  templateUrl: './airport-new.component.html',
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
