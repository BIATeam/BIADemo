import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import {
  BiaButtonGroupComponent,
  BiaButtonGroupItem,
} from 'src/app/shared/bia-shared/components/bia-button-group/bia-button-group.component';
import { BiaTableBehaviorControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableHeaderComponent } from 'src/app/shared/bia-shared/components/table/bia-table-header/bia-table-header.component';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { TeamAdvancedFilterComponent } from 'src/app/shared/bia-shared/components/team-advanced-filter/team-advanced-filter.component';
import { CrudItemService } from 'src/app/shared/bia-shared/feature-templates/crud-items/services/crud-item.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { TeamAdvancedFilterDto } from 'src/app/shared/bia-shared/model/team-advanced-filter-dto';
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
    NgIf,
    BiaButtonGroupComponent,
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

  protected initSelectedButtonGroup() {
    this.selectedButtonGroup = [
      new BiaButtonGroupItem(
        this.translateService.instant('maintenanceTeam.edit'),
        () => this.onEdit(this.selectedCrudItems[0].id),
        this.canEdit,
        this.selectedCrudItems.length !== 1,
        this.translateService.instant('maintenanceTeam.edit')
      ),
      // BIAToolKit - Begin MaintenanceTeamIndexTsChildTeamButton
      // BIAToolKit - End MaintenanceTeamIndexTsChildTeamButton
      new BiaButtonGroupItem(
        this.translateService.instant('app.members'),
        () => this.onViewMembers(this.selectedCrudItems[0].id),
        this.canViewMembers,
        this.selectedCrudItems.length !== 1 ||
          !this.selectedCrudItems[0].canMemberListAccess,
        this.translateService.instant('app.members')
      ),
    ];
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
}
