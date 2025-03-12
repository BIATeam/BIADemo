import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { Notification } from '../../model/notification';
import { notificationCRUDConfiguration } from '../../notification.constants';
import { NotificationService } from '../../services/notification.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { NotificationFormComponent } from '../../components/notification-form/notification-form.component';
import { BiaSharedModule } from '../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'bia-notification-edit',
    templateUrl: './notification-edit.component.html',
    styleUrls: ['./notification-edit.component.scss'],
    imports: [NgIf, NotificationFormComponent, BiaSharedModule, AsyncPipe]
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
