import { AsyncPipe, NgClass, NgIf } from '@angular/common';
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
import { ButtonDirective } from 'primeng/button';
import { Permission } from 'src/app/shared/permission';
import { PilotTableComponent } from '../../components/pilot-table/pilot-table.component';
import { Pilot } from '../../model/pilot';
import { pilotCRUDConfiguration } from '../../pilot.constants';
import { PilotOptionsService } from '../../services/pilot-options.service';
import { PilotService } from '../../services/pilot.service';

@Component({
  selector: 'app-pilots-index',
  templateUrl: './pilots-index.component.html',
  styleUrls: ['./pilots-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    NgIf,
    ButtonDirective,
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
  extends CrudItemsIndexComponent<Pilot>
  implements OnInit
{
  @ViewChild(PilotTableComponent, { static: false })
  crudItemTableComponent: PilotTableComponent;

  // BIAToolKit - Begin PilotIndexTsCanViewChildDeclaration
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial PilotIndexTsCanViewChildDeclaration Engine
  canViewEngines = false;
  // BIAToolKit - End Partial PilotIndexTsCanViewChildDeclaration Engine
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End PilotIndexTsCanViewChildDeclaration

  constructor(
    protected injector: Injector,
    public pilotService: PilotService,
    protected pilotOptionsService: PilotOptionsService,
    protected authService: AuthService
  ) {
    super(injector, pilotService);
    this.crudConfiguration = pilotCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Pilot_Update);
    this.canDelete = this.authService.hasPermission(Permission.Pilot_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Pilot_Create);
    this.canSave = this.authService.hasPermission(Permission.Pilot_Save);
    this.canSelect = this.canDelete;
    this.canFix = this.authService.hasPermission(Permission.Pilot_Fix);
    // BIAToolKit - Begin PilotIndexTsCanViewChildSet
    // Begin BIAToolKit Generation Ignore
    // BIAToolKit - Begin Partial PilotIndexTsCanViewChildSet Engine
    this.canViewEngines = this.authService.hasPermission(
      Permission.Engine_List_Access
    );
    this.canSelect = this.canSelect || this.canViewEngines;
    // BIAToolKit - End Partial PilotIndexTsCanViewChildSet Engine
    // End BIAToolKit Generation Ignore
    // BIAToolKit - End PilotIndexTsCanViewChildSet
  }
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial PilotIndexTsOnViewChild Engine
  onViewEngines(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'engines'], {
        relativeTo: this.activatedRoute,
      });
    }
  }
  // BIAToolKit - End Partial PilotIndexTsOnViewChild Engine
  // End BIAToolKit Generation Ignore

  // BIAToolKit - Begin PilotIndexTsOnViewChild
  // BIAToolKit - End PilotIndexTsOnViewChild
}
