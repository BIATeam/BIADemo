import { Component, Injector } from '@angular/core';
import { CrudItemImportComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-import/crud-item-import.component';
import { Permission } from 'src/app/shared/permission';
import { memberCRUDConfiguration } from '../../member.constants';
import { Member } from '../../model/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'bia-member-import',
  templateUrl:
    '../../../crud-items/views/crud-item-import/crud-item-import.component.html',
})
export class MemberImportComponent extends CrudItemImportComponent<Member> {
  constructor(
    protected injector: Injector,
    protected memberService: MemberService
  ) {
    super(injector, memberService);
    this.crudConfiguration = memberCRUDConfiguration;
    this.setPermissions();
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

  save(toSaves: Member[]): void {
    this.memberService.save(toSaves);
  }
}
