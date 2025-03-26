import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { NotificationFormComponent } from '../../components/notification-form/notification-form.component';
import { Notification } from '../../model/notification';
import { notificationCRUDConfiguration } from '../../notification.constants';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'bia-notification-edit',
  templateUrl: './notification-edit.component.html',
  styleUrls: ['./notification-edit.component.scss'],
  imports: [NgIf, NotificationFormComponent, AsyncPipe, SpinnerComponent],
})
export class NotificationEditComponent extends CrudItemEditComponent<Notification> {
  constructor(
    protected injector: Injector,
    public notificationService: NotificationService
  ) {
    super(injector, notificationService);
    this.crudConfiguration = notificationCRUDConfiguration;
  }
}
