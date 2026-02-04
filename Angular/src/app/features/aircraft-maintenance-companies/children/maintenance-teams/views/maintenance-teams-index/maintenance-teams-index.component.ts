import { AsyncPipe, NgClass } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '@bia-team/bia-ng/core';
import { TeamAdvancedFilterDto } from '@bia-team/bia-ng/models';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
  TeamAdvancedFilterComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';
import { MaintenanceTeamTableComponent } from '../../components/maintenance-team-table/maintenance-team-table.component';
import { maintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-teams-index',
  templateUrl: './maintenance-teams-index.component.html',
  styleUrls: ['./maintenance-teams-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    MaintenanceTeamTableComponent,
    AsyncPipe,
    TranslateModule,
    TeamAdvancedFilterComponent,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [
    { provide: CrudItemService, useExisting: MaintenanceTeamService },
  ],
})
export class MaintenanceTeamsIndexComponent
  extends CrudItemsIndexComponent<MaintenanceTeam>
  implements OnInit
{
  @ViewChild(MaintenanceTeamTableComponent, { static: false })
  crudItemTableComponent: MaintenanceTeamTableComponent;

  // Customization for teams
  canViewMembers = false;
  // BIAToolKit - Begin MaintenanceTeamIndexTsCanViewChildDeclaration
  // BIAToolKit - End MaintenanceTeamIndexTsCanViewChildDeclaration

  checkhasAdvancedFilter() {
    this.hasAdvancedFilter = TeamAdvancedFilterDto.hasFilter(
      this.crudConfiguration.fieldsConfig.advancedFilter
    );
  }

  constructor(
    protected injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
    protected maintenanceTeamOptionsService: MaintenanceTeamOptionsService,
    protected authService: AuthService
  ) {
    super(injector, maintenanceTeamService);
    this.crudConfiguration = maintenanceTeamCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();

    const aircraftMaintenanceCompanyId = this.authService.getCurrentTeam(
      TeamTypeId.AircraftMaintenanceCompany
    )?.teamId;
    if (aircraftMaintenanceCompanyId) {
      this.maintenanceTeamService.aircraftMaintenanceCompanyService.currentCrudItemId =
        aircraftMaintenanceCompanyId;
      this.parentDisplayItemName$ =
        this.maintenanceTeamService.aircraftMaintenanceCompanyService.displayItemName$;
    }
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.MaintenanceTeam_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.MaintenanceTeam_Delete
    );
    this.canAdd = this.authService.hasPermission(
      Permission.MaintenanceTeam_Create
    );
    this.canFix = this.authService.hasPermission(
      Permission.MaintenanceTeam_Fix
    );
    // BIAToolKit - Begin MaintenanceTeamIndexTsCanViewChildSet
    // BIAToolKit - End MaintenanceTeamIndexTsCanViewChildSet
    this.canViewMembers = this.authService.hasPermission(
      Permission.MaintenanceTeam_Member_List_Access
    );
    this.canSelect =
      // BIAToolKit - Begin MaintenanceTeamIndexTsCanSelectElementChildSet
      // BIAToolKit - End MaintenanceTeamIndexTsCanSelectElementChildSet
      this.canViewMembers || this.canDelete;
  }

  onClickRowData(crudItem: MaintenanceTeam) {
    if (crudItem.canMemberListAccess) {
      this.onViewMembers(crudItem.id);
    }
  }

  onViewMembers(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'members'], {
        relativeTo: this.activatedRoute,
      });
    }
  }

  onSelectedElementsChanged(crudItems: MaintenanceTeam[]) {
    super.onSelectedElementsChanged(crudItems);
    if (crudItems.length === 1) {
      this.maintenanceTeamService.currentCrudItemId = crudItems[0].id;
    }
  }

  onDelete(): void {
    super.onDelete();
    this.authService.reLogin();
  }

  // BIAToolKit - Begin MaintenanceTeamIndexTsOnViewChild
  // BIAToolKit - End MaintenanceTeamIndexTsOnViewChild

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      { separator: true },
      {
        label: this.translateService.instant('maintenanceTeam.edit'),
        command: () => this.onEdit(this.selectedCrudItems[0].id),
        visible: this.canEdit,
        disabled: this.selectedCrudItems.length !== 1,
        tooltip: this.translateService.instant('maintenanceTeam.edit'),
        buttonOutlined: true,
      },
      // BIAToolKit - Begin MaintenanceTeamIndexTsSelectedButtonViewChild
      // BIAToolKit - End MaintenanceTeamIndexTsSelectedButtonViewChild
      {
        label: this.translateService.instant('app.members'),
        command: () => this.onViewMembers(this.selectedCrudItems[0].id),
        visible: this.canViewMembers,
        disabled:
          this.selectedCrudItems.length !== 1 ||
          !this.selectedCrudItems[0].canMemberListAccess,
        tooltip: this.translateService.instant('app.members'),
        buttonOutlined: true,
      },
    ];
  }
}
