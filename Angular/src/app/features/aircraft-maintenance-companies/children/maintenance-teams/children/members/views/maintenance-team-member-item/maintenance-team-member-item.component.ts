import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  MemberItemComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';

@Component({
  selector: 'app-maintenance-team-members-item',
  templateUrl:
    '../../../../../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../../../../../packages/bia-ng/shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
})
export class MaintenanceTeamMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) {
    super(injector);
  }
}
