import { AsyncPipe, NgClass, NgIf } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from 'packages/bia-ng/core/public-api';
import { TeamAdvancedFilterDto } from 'packages/bia-ng/models/public-api';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
  TeamAdvancedFilterComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { Permission } from 'src/app/shared/permission';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../../aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompanyTableComponent } from '../../components/aircraft-maintenance-company-table/aircraft-maintenance-company-table.component';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-companies-index',
  templateUrl: './aircraft-maintenance-companies-index.component.html',
  styleUrls: ['./aircraft-maintenance-companies-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    NgIf,
    AircraftMaintenanceCompanyTableComponent,
    AsyncPipe,
    TranslateModule,
    TeamAdvancedFilterComponent,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
  ],
  providers: [
    {
      provide: CrudItemService,
      useExisting: AircraftMaintenanceCompanyService,
    },
  ],
})
export class AircraftMaintenanceCompaniesIndexComponent
  extends CrudItemsIndexComponent<AircraftMaintenanceCompany>
  implements OnInit
{
  @ViewChild(AircraftMaintenanceCompanyTableComponent, { static: false })
  crudItemTableComponent: AircraftMaintenanceCompanyTableComponent;

  // Customization for teams
  canViewMembers = false;
  // BIAToolKit - Begin AircraftMaintenanceCompanyIndexTsCanViewChildDeclaration
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial AircraftMaintenanceCompanyIndexTsCanViewChildDeclaration MaintenanceTeam
  canViewMaintenanceTeams = false;
  // BIAToolKit - End Partial AircraftMaintenanceCompanyIndexTsCanViewChildDeclaration MaintenanceTeam
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End AircraftMaintenanceCompanyIndexTsCanViewChildDeclaration

  checkhasAdvancedFilter() {
    this.hasAdvancedFilter = TeamAdvancedFilterDto.hasFilter(
      this.crudConfiguration.fieldsConfig.advancedFilter
    );
  }

  constructor(
    protected injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
    protected authService: AuthService
  ) {
    super(injector, aircraftMaintenanceCompanyService);
    this.crudConfiguration = aircraftMaintenanceCompanyCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.AircraftMaintenanceCompany_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.AircraftMaintenanceCompany_Delete
    );
    this.canAdd = this.authService.hasPermission(
      Permission.AircraftMaintenanceCompany_Create
    );
    // BIAToolKit - Begin AircraftMaintenanceCompanyIndexTsCanViewChildSet
    // Begin BIAToolKit Generation Ignore
    // BIAToolKit - Begin Partial AircraftMaintenanceCompanyIndexTsCanViewChildSet MaintenanceTeam
    this.canViewMaintenanceTeams = this.authService.hasPermission(
      Permission.MaintenanceTeam_List_Access
    );
    // BIAToolKit - End Partial AircraftMaintenanceCompanyIndexTsCanViewChildSet MaintenanceTeam
    // End BIAToolKit Generation Ignore
    // BIAToolKit - End AircraftMaintenanceCompanyIndexTsCanViewChildSet
    this.canViewMembers = this.authService.hasPermission(
      Permission.AircraftMaintenanceCompany_Member_List_Access
    );
    this.canSelect =
      // BIAToolKit - Begin AircraftMaintenanceCompanyIndexTsCanSelectElementChildSet
      // Begin BIAToolKit Generation Ignore
      // BIAToolKit - Begin Partial AircraftMaintenanceCompanyIndexTsCanSelectElementChildSet MaintenanceTeam
      this.canViewMaintenanceTeams ||
      // BIAToolKit - End Partial AircraftMaintenanceCompanyIndexTsCanSelectElementChildSet MaintenanceTeam
      // End BIAToolKit Generation Ignore
      // BIAToolKit - End AircraftMaintenanceCompanyIndexTsCanSelectElementChildSet
      this.canViewMembers ||
      this.canDelete;
  }

  protected initSelectedButtonGroup() {
    this.selectionActionsMenuItems = [
      { separator: true },
      {
        label: this.translateService.instant('aircraftMaintenanceCompany.edit'),
        command: () => this.onEdit(this.selectedCrudItems[0].id),
        visible: this.canEdit,
        disabled: this.selectedCrudItems.length !== 1,
        tooltip: this.translateService.instant(
          'aircraftMaintenanceCompany.edit'
        ),
      },
      // BIAToolKit - Begin AircraftMaintenanceCompanyIndexTsChildTeamButton
      // Begin BIAToolKit Generation Ignore
      // BIAToolKit - Begin Partial AircraftMaintenanceCompanyIndexTsChildTeamButton MaintenanceTeam
      {
        label: this.translateService.instant(
          'aircraftMaintenanceCompany.maintenanceTeams'
        ),
        command: () => this.onViewMaintenanceTeams(),
        visible: this.canViewMaintenanceTeams,
        disabled: this.selectedCrudItems.length !== 1,
        tooltip: this.translateService.instant(
          'aircraftMaintenanceCompany.maintenanceTeams'
        ),
      },
      // BIAToolKit - End Partial AircraftMaintenanceCompanyIndexTsChildTeamButton MaintenanceTeam
      // End BIAToolKit Generation Ignore
      // BIAToolKit - End AircraftMaintenanceCompanyIndexTsChildTeamButton
      {
        label: this.translateService.instant('app.members'),
        command: () => this.onViewMembers(this.selectedCrudItems[0].id),
        visible: this.canViewMembers,
        disabled:
          this.selectedCrudItems.length !== 1 ||
          !this.selectedCrudItems[0].canMemberListAccess,
        tooltip: this.translateService.instant('app.members'),
      },
    ];
  }

  onClickRowData(crudItem: AircraftMaintenanceCompany) {
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

  onSelectedElementsChanged(crudItems: AircraftMaintenanceCompany[]) {
    super.onSelectedElementsChanged(crudItems);
    if (crudItems.length === 1) {
      this.aircraftMaintenanceCompanyService.currentCrudItemId =
        crudItems[0].id;
    }
  }

  onDelete(): void {
    super.onDelete();
    this.authService.reLogin();
  }

  // BIAToolKit - Begin AircraftMaintenanceCompanyIndexTsOnViewChild
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial AircraftMaintenanceCompanyIndexTsOnViewChild MaintenanceTeam
  onViewMaintenanceTeams() {
    if (this.selectedCrudItems.length === 1) {
      this.router.navigate(
        [this.selectedCrudItems[0].id, 'maintenance-teams'],
        { relativeTo: this.activatedRoute }
      );
    }
  }
  // BIAToolKit - End Partial AircraftMaintenanceCompanyIndexTsOnViewChild MaintenanceTeam
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End AircraftMaintenanceCompanyIndexTsOnViewChild
}
