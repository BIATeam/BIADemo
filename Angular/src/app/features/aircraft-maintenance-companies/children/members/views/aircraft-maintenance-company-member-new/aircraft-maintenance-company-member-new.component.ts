import { Component, Injector, OnInit } from '@angular/core';
import { AircraftMaintenanceCompanyService } from 'src/app/features/aircraft-maintenance-companies/services/aircraft-maintenance-company.service';
import { MemberNewComponent } from 'src/app/shared/bia-shared/features/members/views/member-new/member-new.component';

@Component({
  selector: 'app-aircraft-maintenance-company-member-new',
  templateUrl: '../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/features/members/views/member-new/member-new.component.scss']
})
export class AircraftMaintenanceCompanyMemberNewComponent extends MemberNewComponent implements OnInit {

  constructor(
    injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
  ) {
    super(injector);
  }

  ngOnInit() {
    if (this.aircraftMaintenanceCompanyService.currentAircraftMaintenanceCompany!= null) this.teamId = this.aircraftMaintenanceCompanyService.currentAircraftMaintenanceCompany.id;
    this.teamType=1;
    super.ngOnInit();
  }
}
