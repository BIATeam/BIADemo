import { Component, Injector } from '@angular/core';
import { MemberItemComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-item/member-item.component';

@Component({
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/members/views/member-item/member-item.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/members/views/member-item/member-item.component.scss']
})
export class AircraftMaintenanceCompanyMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) { 
      super (injector);
    }
}
