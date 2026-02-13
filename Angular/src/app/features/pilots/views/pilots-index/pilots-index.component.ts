import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from 'packages/bia-ng/core/public-api';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { Permission } from 'src/app/shared/permission';
import { PilotTableComponent } from '../../components/pilot-table/pilot-table.component';
import { Pilot } from '../../model/pilot';
import { PilotList } from '../../model/pilot-list';
import { pilotListCRUDConfiguration } from '../../pilot-list.constants';
import { PilotService } from '../../services/pilot.service';

@Component({
  selector: 'app-pilots-index',
  templateUrl: './pilots-index.component.html',
  styleUrls: ['./pilots-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    PilotTableComponent,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [{ provide: CrudItemService, useExisting: PilotService }],
})
export class PilotsIndexComponent
  extends CrudItemsIndexComponent<PilotList, Pilot>
  implements OnInit
{
  @ViewChild(PilotTableComponent, { static: false })
  crudItemTableComponent: PilotTableComponent;

  // BIAToolKit - Begin PilotIndexTsCanViewChildDeclaration
  // BIAToolKit - End PilotIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public pilotService: PilotService,
    protected authService: AuthService
  ) {
    super(injector, pilotService);
    this.crudConfiguration = pilotListCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Pilot_Update);
    this.canDelete = this.authService.hasPermission(Permission.Pilot_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Pilot_Create);
    this.canSave = this.authService.hasPermission(Permission.Pilot_Save);
    this.canSelect = this.canDelete;
    this.canFix = this.authService.hasPermission(Permission.Pilot_Fix);
    // BIAToolKit - Begin PilotIndexTsCanViewChildSet
    // BIAToolKit - End PilotIndexTsCanViewChildSet
  }

  // BIAToolKit - Begin PilotIndexTsOnViewChild
  // BIAToolKit - End PilotIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      // BIAToolKit - Begin PilotIndexTsSelectedButtonViewChild
      // BIAToolKit - End PilotIndexTsSelectedButtonViewChild
    ];
  }
}
