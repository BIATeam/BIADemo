import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { airportCRUDConfiguration } from '../../airport.constants';
import { Airport } from '../../model/airport';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-airport-edit',
  templateUrl: './airport-edit.component.html',
})
export class AirportEditComponent extends CrudItemEditComponent<Airport> {
  constructor(
    protected injector: Injector,
    public airportService: AirportService
  ) {
    super(injector, airportService);
    this.crudConfiguration = airportCRUDConfiguration;
  }
}
