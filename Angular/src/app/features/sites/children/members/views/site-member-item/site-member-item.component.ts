import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MemberItemComponent, SpinnerComponent } from '@bia-team/bia-ng/shared';

@Component({
  selector: 'app-site-members-item',
  templateUrl:
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
})
export class SiteMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) {
    super(injector);
  }
}
