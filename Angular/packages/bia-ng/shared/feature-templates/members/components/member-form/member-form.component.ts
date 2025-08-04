import { Component } from '@angular/core';
import { BiaFormComponent } from '../../../../components/form/bia-form/bia-form.component';
import { CrudItemFormComponent } from '../../../crud-items/components/crud-item-form/crud-item-form.component';
import { Member } from '../../model/member';

@Component({
  selector: 'bia-member-form',
  templateUrl:
    '../../../crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class MemberFormComponent extends CrudItemFormComponent<Member> {}
