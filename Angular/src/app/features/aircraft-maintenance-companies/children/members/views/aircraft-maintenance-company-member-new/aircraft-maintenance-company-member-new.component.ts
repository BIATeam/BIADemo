import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  MemberFormNewComponent,
  MemberModule,
  MemberNewComponent,
} from '@bia-team/bia-ng/shared';
import { TeamTypeId } from 'src/app/shared/constants';
import { AircraftMaintenanceCompanyService } from '../../../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-company-member-new',
  templateUrl:
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/members/views/member-new/member-new.component.html',
  imports: [MemberModule, AsyncPipe, MemberFormNewComponent],
})
export class AircraftMaintenanceCompanyMemberNewComponent
  extends MemberNewComponent
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
