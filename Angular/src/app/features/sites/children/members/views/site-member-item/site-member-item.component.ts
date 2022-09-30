import { Component, Injector } from '@angular/core';
import { MemberItemComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-item/member-item.component';

@Component({
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss']
})
export class SiteMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) { 
      super (injector);
    }
}
