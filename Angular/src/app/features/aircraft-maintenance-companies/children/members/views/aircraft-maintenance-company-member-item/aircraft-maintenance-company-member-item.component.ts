import { Component, Injector } from '@angular/core';
import { MemberItemComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-item/member-item.component';

@Component({
    selector: 'app-aircraft-maintenance-company-members-item',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
    styleUrls: [
        '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
    ],
    standalone: false
})
export class AircraftMaintenanceCompanyMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) {
    super(injector);
  }
}
