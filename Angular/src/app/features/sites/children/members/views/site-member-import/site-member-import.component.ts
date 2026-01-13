import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  BiaFormComponent,
  CrudItemImportFormComponent,
  MemberImportComponent,
  MemberService,
} from '@bia-team/bia-ng/shared';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'app-site-member-import',
  templateUrl:
    '../../../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-import/crud-item-import.component.html',
  imports: [CrudItemImportFormComponent, AsyncPipe, BiaFormComponent],
})
export class SiteMemberImportComponent extends MemberImportComponent {
  constructor(
    protected injector: Injector,
    protected memberService: MemberService
  ) {
    super(injector, memberService);
  }

  setPermissions() {
    this.canEdit = this.authService.hasPermission(
      Permission.Site_Member_Update
    );
    this.canDelete = this.authService.hasPermission(
      Permission.Site_Member_Delete
    );
    this.canAdd = this.authService.hasPermission(Permission.Site_Member_Create);
  }
}
