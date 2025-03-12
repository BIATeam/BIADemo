import { Component, Injector, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaButtonGroupItem } from 'src/app/shared/bia-shared/components/bia-button-group/bia-button-group.component';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { TeamAdvancedFilterDto } from 'src/app/shared/bia-shared/model/team-advanced-filter-dto';
import { Permission } from 'src/app/shared/permission';
import { aircraftMaintenanceCompanyCRUDConfiguration } from '../../aircraft-maintenance-company.constants';
import { AircraftMaintenanceCompanyTableComponent } from '../../components/aircraft-maintenance-company-table/aircraft-maintenance-company-table.component';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { AircraftMaintenanceCompanyService } from '../../services/aircraft-maintenance-company.service';
import { NgIf, NgClass, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';
import { PrimeTemplate } from 'primeng/api';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-aircraft-maintenance-companies-index',
    templateUrl: './aircraft-maintenance-companies-index.component.html',
    styleUrls: ['./aircraft-maintenance-companies-index.component.scss'],
    imports: [NgIf, BiaSharedModule, NgClass, PrimeTemplate, AircraftMaintenanceCompanyTableComponent, AsyncPipe, TranslateModule]
})
export class AircraftMaintenanceCompaniesIndexComponent extends CrudItemsIndexComponent<AircraftMaintenanceCompany> {
  // Custo for teams
  canViewMembers = false;
  canSelectElement = false;
  // Begin Child MaintenanceTeam
  canViewMaintenanceTeams = false;
  // End Child MaintenanceTeam

  checkhasAdvancedFilter() {
    this.hasAdvancedFilter = TeamAdvancedFilterDto.hasFilter(
      this.crudConfiguration.fieldsConfig.advancedFilter
    );
  }

  @ViewChild(AircraftMaintenanceCompanyTableComponent, { static: false })
  crudItemTableComponent: AircraftMaintenanceCompanyTableComponent;

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
    // Custo for teams
    this.canViewMembers = this.authService.hasPermission(
      Permission.AircraftMaintenanceCompany_Member_List_Access
    );
    // Begin Child MaintenanceTeam
    this.canViewMaintenanceTeams = this.authService.hasPermission(
      Permission.MaintenanceTeam_List_Access
    );
    // End Child MaintenanceTeam
    this.canSelectElement =
      // Begin Child MaintenanceTeam
      this.canViewMaintenanceTeams ||
      // End Child MaintenanceTeam
      this.canViewMembers ||
      this.canDelete;
  }

  protected initSelectedButtonGroup() {
    this.selectedButtonGroup = [
      new BiaButtonGroupItem(
        this.translateService.instant('aircraftMaintenanceCompany.edit'),
        () => this.onEdit(this.selectedCrudItems[0].id),
        this.canEdit,
        this.selectedCrudItems.length !== 1,
        this.translateService.instant('aircraftMaintenanceCompany.edit')
      ),
      // Begin Child MaintenanceTeam
      new BiaButtonGroupItem(
        this.translateService.instant(
          'aircraftMaintenanceCompany.maintenanceTeams'
        ),
        () => this.onViewMaintenanceTeams(),
        this.canViewMaintenanceTeams,
        this.selectedCrudItems.length !== 1,
        this.translateService.instant(
          'aircraftMaintenanceCompany.maintenanceTeams'
        )
      ),
      new BiaButtonGroupItem(
        this.translateService.instant('app.members'),
        () => this.onViewMembers(this.selectedCrudItems[0].id),
        this.canViewMembers,
        this.selectedCrudItems.length !== 1 ||
          !this.selectedCrudItems[0].canMemberListAccess,
        this.translateService.instant('app.members')
      ),
      // End Child MaintenanceTeam
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

  // Begin Child MaintenanceTeam
  onViewMaintenanceTeams() {
    if (this.selectedCrudItems.length === 1) {
      this.router.navigate(
        [this.selectedCrudItems[0].id, 'maintenance-teams'],
        { relativeTo: this.activatedRoute }
      );
    }
  }
  // End Child MaintenanceTeam
}
