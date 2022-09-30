import { Component, Injector, OnInit } from '@angular/core';
import { AircraftMaintenanceCompanyService } from '../../../../services/aircraft-maintenance-company.service';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-aircraft-maintenance-company-members-index',
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/members-index/members-index.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component.scss']

})
export class AircraftMaintenanceCompanyMembersIndexComponent extends MembersIndexComponent implements OnInit {
  constructor(
    injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId=TeamTypeId.AircraftMaintenanceCompany;
    super.ngOnInit();
    this.parentIds = [this.aircraftMaintenanceCompanyService.currentCrudItemId?.toString()];
  }
  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_Update);
    this.canDelete = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_Delete);
    this.canAdd = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_Create);
  }
}
