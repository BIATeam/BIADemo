import { Component, Injector, OnInit } from '@angular/core';
import { AircraftMaintenanceCompanyService } from 'src/app/features/aircraft-maintenance-companies/services/aircraft-maintenance-company.service';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-aircraft-maintenance-company-member-edit',
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.scss']
})
export class AircraftMaintenanceCompanyMemberEditComponent extends MemberEditComponent implements OnInit {
  constructor(
    injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
  ) { 
    super(injector);
  }

  ngOnInit() {
    this.teamId = this.aircraftMaintenanceCompanyService.currentAircraftMaintenanceCompanyId;
    this.teamTypeId=TeamTypeId.AircraftMaintenanceCompany;
    super.ngOnInit();
  }
}
