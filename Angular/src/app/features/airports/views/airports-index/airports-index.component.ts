import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, ViewChild } from '@angular/core';
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
import { airportCRUDConfiguration } from '../../airport.constants';
import { AirportTableComponent } from '../../components/airport-table/airport-table.component';
import { Airport } from '../../model/airport';
import { AirportService } from '../../services/airport.service';

@Component({
  selector: 'app-airports-index',
  templateUrl: './airports-index.component.html',
  styleUrls: ['./airports-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    AirportTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [
    {
      provide: CrudItemService,
      useExisting: AirportService,
    },
  ],
})
export class AirportsIndexComponent extends CrudItemsIndexComponent<Airport> {
  @ViewChild(AirportTableComponent, { static: false })
  crudItemTableComponent: AirportTableComponent;
  // BIAToolKit - Begin AirportIndexTsCanViewChildDeclaration
  // BIAToolKit - End AirportIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public airportService: AirportService,
    protected authService: AuthService
  ) {
    super(injector, airportService);
    this.crudConfiguration = airportCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Airport_Update);
    this.canDelete = this.authService.hasPermission(Permission.Airport_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Airport_Create);
    // BIAToolKit - Begin MaintenanceContractIndexTsCanViewChildSet
    // BIAToolKit - End MaintenanceContractIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin MaintenanceContractIndexTsOnViewChild
  // BIAToolKit - End MaintenanceContractIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      // BIAToolKit - Begin AirportIndexTsSelectedButtonViewChild
      // BIAToolKit - End AirportIndexTsSelectedButtonViewChild
    ];
  }
}
