import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MemberItemComponent, SpinnerComponent } from 'bia-ng/shared';

@Component({
  selector: 'app-maintenance-team-members-item',
  templateUrl:
    '../../../../../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../../../../../node_modules/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
})
export class MaintenanceTeamMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) {
    super(injector);
  }
}
