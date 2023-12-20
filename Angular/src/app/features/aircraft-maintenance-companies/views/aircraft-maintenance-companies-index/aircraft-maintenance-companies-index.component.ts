import { Component, Injector, ViewChild } from '@angular/core';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyCRUDConfiguration } from '../../aircraft-maintenance-company.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';
import { AircraftMaintenanceCompanyTableComponent } from '../../components/aircraft-maintenance-company-table/aircraft-maintenance-company-table.component';
import { TeamAdvancedFilterDto } from 'src/app/shared/bia-shared/model/team-advanced-filter-dto';

@Component({
  selector: 'app-aircraft-maintenance-companies-index',
  templateUrl: './aircraft-maintenance-companies-index.component.html',
  styleUrls: ['./aircraft-maintenance-companies-index.component.scss']
})

export class AircraftMaintenanceCompaniesIndexComponent extends CrudItemsIndexComponent<AircraftMaintenanceCompany> {
  // Custo for teams
  canViewMembers = false;
  canSelectElement = false;
  // Begin Child MaintenanceTeam
  canViewMaintenanceTeams = false;
  // End Child MaintenanceTeam

  checkhasAdvancedFilter()
  {
    this.hasAdvancedFilter =  TeamAdvancedFilterDto.hasFilter(this.crudConfiguration.fieldsConfig.advancedFilter);
  }

  @ViewChild(AircraftMaintenanceCompanyTableComponent, { static: false }) crudItemTableComponent: AircraftMaintenanceCompanyTableComponent;

  constructor(
    protected injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
    protected authService: AuthService,
  ) {
    super(injector, aircraftMaintenanceCompanyService);
    this.crudConfiguration = AircraftMaintenanceCompanyCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Update);
    this.canDelete = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Delete);
    this.canAdd = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Create);
    // Custo for teams
    this.canViewMembers = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_List_Access);
    // Begin Child MaintenanceTeam
    this.canViewMaintenanceTeams = this.authService.hasPermission(Permission.MaintenanceTeam_List_Access);;
    // End Child MaintenanceTeam
    this.canSelectElement = 
      // Begin Child MaintenanceTeam
      this.canViewMaintenanceTeams ||
      // End Child MaintenanceTeam
      this.canViewMembers ||
      this.canDelete;

  }
  
  onClickRowData(crudItem: AircraftMaintenanceCompany) {
    if (crudItem.canMemberListAccess) {
      this.onViewMembers(crudItem.id);
    }
  }

  onViewMembers(crudItemId: any) {
    if (crudItemId && crudItemId > 0) {
      this.router.navigate([crudItemId, 'members'], { relativeTo: this.activatedRoute });
    }
  }

  // Begin Child MaintenanceTeam
  onViewMaintenanceTeams() {
    if (this.selectedCrudItems.length == 1) {
      this.router.navigate([this.selectedCrudItems[0].id, 'maintenance-teams'], { relativeTo: this.activatedRoute });
    }
  }
  // End Child MaintenanceTeam
}
