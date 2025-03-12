import { Component, Injector, OnInit } from '@angular/core';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { AircraftMaintenanceCompanyService } from '../../../../services/aircraft-maintenance-company.service';
import { MemberFormEditComponent } from '../../../../../../shared/bia-shared/feature-templates/members/components/member-form-edit/member-form-edit.component';
import { NgIf, AsyncPipe } from '@angular/common';
import { SpinnerComponent } from '../../../../../../shared/bia-shared/components/spinner/spinner.component';

@Component({
    selector: 'app-aircraft-maintenance-company-member-edit',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
    imports: [MemberFormEditComponent, NgIf, SpinnerComponent, AsyncPipe]
})
export class AircraftMaintenanceCompanyMemberEditComponent
  extends MemberEditComponent
  implements OnInit
{
  constructor(
    injector: Injector,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.teamTypeId = TeamTypeId.AircraftMaintenanceCompany;
    super.ngOnInit();
  }
}
