import { Component, OnChanges } from '@angular/core';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { Notification } from '../../model/notification';

@Component({
  selector: 'bia-notification-table',
  templateUrl: '../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class NotificationTableComponent extends BiaCalcTableComponent implements OnChanges {

  constructor(
    public formBuilder: UntypedFormBuilder,
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
      title: [this.element.title, Validators.required],
      description: [this.element.description, Validators.required],
      type: [this.element.type?.id, Validators.required],
      read: [this.element.read],
      createdDate: [this.element.createdDate, Validators.required],
      createdBy: [this.element.createdBy?.id],
      notifiedUsers: [this.element.notifiedUsers],
      jData: [this.element.jData],
    });
  }

    onSubmit() {
    if (this.form.valid) {
      const notification: Notification = <Notification>this.form.value;
      notification.id = notification.id > 0 ? notification.id : 0;
      notification.read = notification.read ? notification.read : false;
      notification.createdBy = BiaOptionService.Clone(notification.createdBy);
      notification.notifiedUsers = BiaOptionService.Differential(notification.notifiedUsers, this.element?.notifiedUsers);
      notification.type = { ...notification.type };
      this.save.emit(notification);
      this.form.reset();
    }
  }
}
