import { Component, Injector, OnInit } from '@angular/core';
import { AircraftMaintenanceCompanyService } from '../../../../services/aircraft-maintenance-company.service';
import { MemberNewComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-new/member-new.component';
import { TeamTypeId } from 'src/app/shared/constants';

@Component({
  selector: 'app-aircraft-maintenance-company-member-new',
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-new/member-new.component.html',
})
export class AircraftMaintenanceCompanyMemberNewComponent extends MemberNewComponent implements OnInit {

  constructor(
    injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId=TeamTypeId.AircraftMaintenanceCompany;
    super.ngOnInit();
  }
}
