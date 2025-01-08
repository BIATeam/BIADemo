import { Component, Injector } from '@angular/core';
import { CrudItemBulkComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-bulk/crud-item-bulk.component';
import { Permission } from 'src/app/shared/permission';
import { memberCRUDConfiguration } from '../../member.constants';
import { Member } from '../../model/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'bia-member-bulk',
  templateUrl:
    '../../../crud-items/views/crud-item-bulk/crud-item-bulk.component.html',
})
export class MemberBulkComponent extends CrudItemBulkComponent<Member> {
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
