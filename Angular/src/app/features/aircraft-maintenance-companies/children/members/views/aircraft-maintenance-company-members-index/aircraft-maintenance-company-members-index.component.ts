import { Component, Injector, OnInit } from '@angular/core';
import { AircraftMaintenanceCompanyService } from 'src/app/features/aircraft-maintenance-companies/services/aircraft-maintenance-company.service';
import { MembersIndexComponent } from 'src/app/shared/bia-shared/features/members/views/members-index/members-index.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-aircraft-maintenance-company-members-index',
  templateUrl: '../../../../../../shared/bia-shared/features/members/views/members-index/members-index.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/features/members/views/members-index/members-index.component.scss']
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
    this.parentIds = ['' + this.aircraftMaintenanceCompanyService.currentAircraftMaintenanceCompanyId];
  }
  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_Update);
    this.canDelete = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_Delete);
    this.canAdd = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_Create);
  }
}
