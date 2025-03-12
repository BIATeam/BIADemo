import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { airportCRUDConfiguration } from '../../airport.constants';
import { Airport } from '../../model/airport';
import { AirportSignalRService } from '../../services/airport-signalr.service';
import { AirportService } from '../../services/airport.service';

@Component({
    selector: 'app-airport-edit',
    templateUrl: './airport-edit.component.html',
    standalone: false
})
export class AirportEditComponent
  extends CrudItemEditComponent<Airport>
  implements OnInit, OnDestroy
{
  private useSignalR = airportCRUDConfiguration.useSignalR;

  constructor(
    protected injector: Injector,
    public airportService: AirportService,
    protected signalrService: AirportSignalRService
  ) {
    super(injector, airportService);
    this.crudConfiguration = airportCRUDConfiguration;
  }

  ngOnInit() {
    super.ngOnInit();

    if (this.useSignalR) {
      this.signalrService.initialize();

      this.signalrService.registerUpdate(async args => {
        const updatedCrudItem = JSON.parse(args) as Airport;
        const currentCrudItem = await firstValueFrom(
          this.airportService.crudItem$
        );

        if (
          currentCrudItem.id === updatedCrudItem.id &&
          currentCrudItem.rowVersion !== updatedCrudItem.rowVersion
        ) {
          this.isCrudItemOutdated = true;
        }
      });
    }
  }

  ngOnDestroy() {
    super.ngOnDestroy();

    if (this.useSignalR) {
      this.signalrService.destroy();
    }
  }
}
