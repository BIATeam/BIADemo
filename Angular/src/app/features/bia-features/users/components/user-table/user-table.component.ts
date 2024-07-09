import { Component } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { CrudItemTableComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-table/crud-item-table.component';
import { User } from '../../model/user';

@Component({
  selector: 'bia-user-table',
  templateUrl:
    '../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
})
export class UserTableComponent extends CrudItemTableComponent<User> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
