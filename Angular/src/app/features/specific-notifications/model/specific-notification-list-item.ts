import {
  NotificationListItem,
  notificationFieldsConfiguration,
} from 'packages/bia-ng/features/public-api';
import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
} from 'packages/bia-ng/models/public-api';

export interface SpecificNotificationListItem extends NotificationListItem {
  acknowledgedAt?: Date;
}

export const specificNotificationFieldsConfiguration: BiaFieldsConfig<SpecificNotificationListItem> =
  {
    columns: [
      ...notificationFieldsConfiguration.columns,
      Object.assign(
        new BiaFieldConfig<SpecificNotificationListItem>(
          'acknowledgedAt',
          'notification.acknowledgedAt'
        ),
        {
          type: PropType.DateTime,
          asLocalDateTime: true,
        }
      ),
    ],
  };
