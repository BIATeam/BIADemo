import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { Component, Injector, OnInit } from '@angular/core';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { AircraftMaintenanceCompanyService } from '../../../../services/aircraft-maintenance-company.service';
import { MemberModule } from '../../../../../../shared/bia-shared/feature-templates/members/member.module';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-aircraft-maintenance-company-member-edit',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
    imports: [MemberModule, NgIf, BiaSharedModule, AsyncPipe, SpinnerComponent]
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
