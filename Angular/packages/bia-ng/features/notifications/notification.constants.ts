import { BiaTeamTypeId } from 'packages/bia-ng/models/enum/public-api';
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
    useViewTeamWithTypeId: BiaTeamTypeId.Site, // use to filter view by teams => should know the type of team
    usePopup: true,
    useOfflineMode: false,
    useCompactMode: false,
    useVirtualScroll: false,
    useRefreshAtLanguageChange: true,
  });
