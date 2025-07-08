import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { NotificationFormComponent } from '../../components/notification-form/notification-form.component';
import { Notification } from '../../model/notification';
import { notificationCRUDConfiguration } from '../../notification.constants';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'bia-notification-new',
  templateUrl: './notification-new.component.html',
  styleUrls: ['./notification-new.component.scss'],
  imports: [NotificationFormComponent, AsyncPipe],
})
export class NotificationNewComponent extends CrudItemNewComponent<Notification> {
  constructor(
    protected injector: Injector,
    public notificationService: NotificationService
  ) {
    super(injector, notificationService);
    this.crudConfiguration = notificationCRUDConfiguration;
  }
}
