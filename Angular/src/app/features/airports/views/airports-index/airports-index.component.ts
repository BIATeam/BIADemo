import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { Component, Injector, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { airportCRUDConfiguration } from '../../airport.constants';
import { AirportTableComponent } from '../../components/airport-table/airport-table.component';
import { Airport } from '../../model/airport';
import { AirportService } from '../../services/airport.service';
import { NgClass, NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';
import { PrimeTemplate } from 'primeng/api';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-airports-index',
    templateUrl: './airports-index.component.html',
    styleUrls: ['./airports-index.component.scss'],
    imports: [NgClass, BiaSharedModule, PrimeTemplate, NgIf, AirportTableComponent, AsyncPipe, TranslateModule, BiaTableHeaderComponent, BiaTableControllerComponent, BiaTableBehaviorControllerComponent, BiaTableComponent]
})
export class AirportsIndexComponent extends CrudItemsIndexComponent<Airport> {
  @ViewChild(AirportTableComponent, { static: false })
  crudItemTableComponent: AirportTableComponent;

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
  }
}
