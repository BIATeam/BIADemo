import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '@bia-team/bia-ng/core';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { Permission } from 'src/app/shared/permission';
import { FlightTableComponent } from '../../components/flight-table/flight-table.component';
import { flightCRUDConfiguration } from '../../flight.constants';
import { Flight } from '../../model/flight';
import { FlightOptionsService } from '../../services/flight-options.service';
import { FlightService } from '../../services/flight.service';

@Component({
  selector: 'app-flights-index',
  templateUrl: './flights-index.component.html',
  styleUrls: ['./flights-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    FlightTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [{ provide: CrudItemService, useExisting: FlightService }],
})
export class FlightsIndexComponent
  extends CrudItemsIndexComponent<Flight>
  implements OnInit
{
  @ViewChild(FlightTableComponent, { static: false })
  crudItemTableComponent: FlightTableComponent;

  // BIAToolKit - Begin FlightIndexTsCanViewChildDeclaration
  // BIAToolKit - End FlightIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public flightService: FlightService,
    protected flightOptionsService: FlightOptionsService,
    protected authService: AuthService
  ) {
    super(injector, flightService);
    this.crudConfiguration = flightCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Flight_Update);
    this.canDelete = this.authService.hasPermission(Permission.Flight_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Flight_Create);
    this.canSave = this.authService.hasPermission(Permission.Flight_Save);
    this.canSelect = this.canDelete;
    this.canFix = this.authService.hasPermission(Permission.Flight_Fix);
    // BIAToolKit - Begin FlightIndexTsCanViewChildSet
    // BIAToolKit - End FlightIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin FlightIndexTsOnViewChild
  // BIAToolKit - End FlightIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      // BIAToolKit - Begin FlightIndexTsSelectedButtonViewChild
      // BIAToolKit - End FlightIndexTsSelectedButtonViewChild
    ];
  }
}
