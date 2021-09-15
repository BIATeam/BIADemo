import { Component, OnChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { Member } from '../../model/member';

@Component({
  selector: 'app-member-table',
  templateUrl: '../../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class MemberTableComponent extends BiaCalcTableComponent implements OnChanges {

  constructor(
    public formBuilder: FormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
    this.initForm();
  }

  public initForm() {
    this.form = this.formBuilder.group({
      id: [this.element.id], // This field is mandatory. Do not remove it.
      user: [this.element.user.id, Validators.required],
      roles: [this.element.roles],
    });
  }

    onSubmit() {
    if (this.form.valid) {
      const member: Member = <Member>this.form.value;
      member.id = member.id > 0 ? member.id : 0;
      member.roles = BiaOptionService.Differential(member.roles, this.element?.roles);
      member.user = {...member.user}
      this.save.emit(member);
      this.form.reset();
    }
  }
}
