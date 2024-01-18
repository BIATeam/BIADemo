import { Component, Injector } from '@angular/core';
import { MemberItemComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-item/member-item.component';

@Component({
  selector: 'app-maintenance-team-members-item',
  templateUrl: '../../../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: ['../../../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss']
})
export class MaintenanceTeamMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) { 
      super (injector);
    }
}
