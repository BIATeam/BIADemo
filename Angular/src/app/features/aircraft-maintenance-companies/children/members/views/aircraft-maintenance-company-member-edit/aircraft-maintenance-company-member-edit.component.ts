﻿import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import {
  MemberEditComponent,
  MemberFormEditComponent,
  MemberModule,
  SpinnerComponent,
} from 'biang/shared';
import { TeamTypeId } from 'src/app/shared/constants';
import { AircraftMaintenanceCompanyService } from '../../../../services/aircraft-maintenance-company.service';

@Component({
  selector: 'app-aircraft-maintenance-company-member-edit',
  templateUrl:
    '../../../../../../../../node_modules/biang/templates/feature-templates/members/views/member-edit/member-edit.component.html',
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
