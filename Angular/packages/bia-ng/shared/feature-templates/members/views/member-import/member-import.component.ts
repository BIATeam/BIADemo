import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { BiaFormComponent } from '../../../../components/form/bia-form/bia-form.component';
import { CrudItemImportFormComponent } from '../../../crud-items/components/crud-item-import-form/crud-item-import-form.component';
import { CrudItemImportComponent } from '../../../crud-items/views/crud-item-import/crud-item-import.component';
import { memberCRUDConfiguration } from '../../member.constants';
import { Member } from '../../model/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'bia-member-import',
  templateUrl:
    '../../../crud-items/views/crud-item-import/crud-item-import.component.html',
  imports: [BiaFormComponent, CrudItemImportFormComponent, AsyncPipe],
})
export abstract class MemberImportComponent extends CrudItemImportComponent<Member> {
  constructor(
    protected injector: Injector,
    protected memberService: MemberService
  ) {
    super(injector, memberService);
    this.crudConfiguration = memberCRUDConfiguration;
    this.setPermissions();
  }

  abstract setPermissions(): void;

  save(toSaves: Member[]): void {
    this.memberService.save(toSaves);
  }
}
