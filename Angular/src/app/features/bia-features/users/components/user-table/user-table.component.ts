import { Component, OnChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { User } from '../../model/user';

@Component({
  selector: 'bia-user-table',
  templateUrl: '../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class UserTableComponent extends BiaCalcTableComponent implements OnChanges {

  constructor(
    public formBuilder: FormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }

  public initForm() {
    this.form = this.formBuilder.group({
      id: [this.element.id], // This field is mandatory. Do not remove it.
      firstName: [this.element.firstName, Validators.required],
      lastName: [this.element.lastName, Validators.required],
      login: [this.element.login, Validators.required],
      roles: [this.element.roles],
    });
  }

    onSubmit() {
    if (this.form.valid) {
      const user: User = <User>this.form.value;
      user.id = user.id > 0 ? user.id : 0;
      user.roles = BiaOptionService.Differential(user.roles, this.element?.roles);

      this.save.emit(user);
      this.form.reset();
    }
  }
}
