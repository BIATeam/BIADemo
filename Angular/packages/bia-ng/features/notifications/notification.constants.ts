import { CrudConfig } from 'packages/bia-ng/shared/public-api';
import {
  notificationFieldsConfiguration,
  NotificationListItem,
} from './model/notification-list-item';

export const notificationCRUDConfiguration: CrudConfig<NotificationListItem> =
  new CrudConfig({
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'notifications',
    fieldsConfig: notificationFieldsConfiguration,
    useCalcMode: false,
    useSignalR: true,
    useView: true,
    usePopup: true,
    useOfflineMode: false,
    useCompactMode: false,
    useVirtualScroll: false,
    useRefreshAtLanguageChange: true,
  });
