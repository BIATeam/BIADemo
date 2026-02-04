import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemEditComponent,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { NotificationFormComponent } from '../../components/notification-form/notification-form.component';
import { Notification } from '../../model/notification';
import { notificationCRUDConfiguration } from '../../notification.constants';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'bia-notification-edit',
  templateUrl: './notification-edit.component.html',
  styleUrls: ['./notification-edit.component.scss'],
  imports: [NotificationFormComponent, AsyncPipe, SpinnerComponent],
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
