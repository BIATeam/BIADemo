import { Component, Injector } from '@angular/core';
import { Airport } from '../../model/airport';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { AirportService } from '../../services/airport.service';
import { AirportCRUDConfiguration } from '../../airport.constants';

@Component({
  selector: 'app-airport-new',
  templateUrl: './airport-new.component.html',
})
export class AirportNewComponent extends CrudItemNewComponent<Airport>  {
   constructor(
    protected injector: Injector,
    public airportService: AirportService,
  ) {
     super(injector, airportService);
     this.crudConfiguration = AirportCRUDConfiguration;
   }
}