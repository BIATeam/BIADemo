import { Component } from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { BiaFormComponent } from '../../../../components/form/bia-form/bia-form.component';
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
