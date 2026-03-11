import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemImportComponent,
  CrudItemImportFormComponent,
} from '@bia-team/bia-ng/shared';
import { Permission } from 'src/app/shared/permission';
import { flightCRUDConfiguration } from '../../flight.constants';
import { Flight } from '../../model/flight';
import { FlightService } from '../../services/flight.service';

@Component({
  selector: 'app-flight-import',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
  imports: [CrudItemImportFormComponent, AsyncPipe, BiaFormComponent],
})
export class FlightImportComponent extends CrudItemImportComponent<Flight> {
  constructor(
    protected injector: Injector,
    private flightService: FlightService
  ) {
    super(injector, flightService);
    this.crudConfiguration = flightCRUDConfiguration;
    this.setPermissions();
  }

  setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Flight_Update);
    this.canDelete = this.authService.hasPermission(Permission.Flight_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Flight_Create);
  }

  save(toSaves: Flight[]): void {
    this.flightService.save(toSaves);
  }
}
