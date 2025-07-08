import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { MemberFormEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/components/member-form-edit/member-form-edit.component';
import { MemberModule } from 'src/app/shared/bia-shared/feature-templates/members/member.module';
import { MemberEditComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component';
import { TeamTypeId } from 'src/app/shared/constants';
import { AircraftMaintenanceCompanyService } from '../../../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-company-member-edit',
  templateUrl:
    '../../../../../../shared/bia-shared/feature-templates/members/views/member-edit/member-edit.component.html',
  imports: [
    MemberModule,
    NgIf,
    AsyncPipe,
    SpinnerComponent,
    MemberFormEditComponent,
  ],
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
