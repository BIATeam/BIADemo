import { TeamTypeId } from 'biang/models/enum';
import { CrudConfig } from 'biang/shared';
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
    useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
    usePopup: true,
    useOfflineMode: false,
    useCompactMode: false,
    useVirtualScroll: false,
  });
