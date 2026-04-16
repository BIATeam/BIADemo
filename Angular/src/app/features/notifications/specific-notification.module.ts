import { NgModule } from '@angular/core';
import {
  BiaNotificationModule,
  NotificationDas,
  NotificationService,
} from 'packages/bia-ng/features/public-api';
import { CrudItemService } from 'packages/bia-ng/shared/public-api';
import { SpecificNotificationDas } from './services/specific-notification-das.service';
import { SpecificNotificationService } from './services/specific-notification.service';
import { specificNotificationCRUDConfiguration } from './specific-notification.constants';
import { SpecificNotificationDetailComponent } from './views/notification-detail/specific-notification-detail.component';

@NgModule({
  imports: [
    BiaNotificationModule.forFeature({
      crudConfiguration: specificNotificationCRUDConfiguration,
      detailComponent: SpecificNotificationDetailComponent,
      providers: [
        { provide: NotificationDas, useClass: SpecificNotificationDas },
        { provide: NotificationService, useClass: SpecificNotificationService },
        { provide: CrudItemService, useExisting: NotificationService },
      ],
    }),
  ],
})
export class SpecificNotificationModule {}
