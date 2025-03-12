import { Component, Injector } from '@angular/core';
import { MemberItemComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-item/member-item.component';
import { RouterOutlet } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-aircraft-maintenance-company-members-item',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
    styleUrls: [
        '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
    ],
    imports: [RouterOutlet, NgIf, BiaSharedModule, AsyncPipe]
})
export class AircraftMaintenanceCompanyMemberItemComponent extends MemberItemComponent {
  constructor(injector: Injector) {
    super(injector);
  }
}
