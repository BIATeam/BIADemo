import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { TeamAdvancedFilterDto } from 'src/app/shared/bia-shared/model/team-advanced-filter-dto';
import { Permission } from 'src/app/shared/permission';
import { MaintenanceTeamTableComponent } from '../../components/maintenance-team-table/maintenance-team-table.component';
import { maintenanceTeamCRUDConfiguration } from '../../maintenance-team.constants';
import { MaintenanceTeam } from '../../model/maintenance-team';
// BIAToolKit - Begin Option
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
// BIAToolKit - End Option
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-teams-index',
  templateUrl: './maintenance-teams-index.component.html',
  styleUrls: ['./maintenance-teams-index.component.scss'],
})
export class MaintenanceTeamsIndexComponent
  extends CrudItemsIndexComponent<MaintenanceTeam>
  implements OnInit
{
  // Custo for teams
  canViewMembers = false;
  canSelectElement = false;

  checkhasAdvancedFilter() {
    this.hasAdvancedFilter = TeamAdvancedFilterDto.hasFilter(
      this.crudConfiguration.fieldsConfig.advancedFilter
    );
  }

  @ViewChild(MaintenanceTeamTableComponent, { static: false })
  crudItemTableComponent: MaintenanceTeamTableComponent;

  constructor(
    protected injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
    // BIAToolKit - Begin Option
    protected maintenanceTeamOptionsService: MaintenanceTeamOptionsService,
    // BIAToolKit - End Option
    protected authService: AuthService
  ) {
    super(injector, maintenanceTeamService);
    this.crudConfiguration = maintenanceTeamCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    // BIAToolKit - Begin Option
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.maintenanceTeamOptionsService.loadAllOptions();
      })
    );
    // BIAToolKit - End Option
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
    // Custo for teams
    this.canViewMembers = this.authService.hasPermission(
      Permission.MaintenanceTeam_Member_List_Access
    );
    this.canSelectElement = this.canViewMembers || this.canDelete;
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
}
