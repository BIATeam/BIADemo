import { Component } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { CrudItemTableComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-table/crud-item-table.component';
import { Member } from '../../model/member';

@Component({
  selector: 'bia-member-table',
  templateUrl:
    '../../../../components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
})
export class MemberTableComponent extends CrudItemTableComponent<Member> {
  constructor(
    protected formBuilder: UntypedFormBuilder,
    protected authService: AuthService,
    protected biaMessageService: BiaMessageService,
    protected translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
