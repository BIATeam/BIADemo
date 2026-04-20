import { CrudConfig } from 'packages/bia-ng/shared/public-api';
import {
  SpecificNotificationListItem,
  specificNotificationFieldsConfiguration,
} from './model/specific-notification-list-item';

export const specificNotificationCRUDConfiguration: CrudConfig<SpecificNotificationListItem> =
  new CrudConfig({
    featureName: 'notifications',
    storeKey: 'notifications',
    fieldsConfig: specificNotificationFieldsConfiguration,
    useCalcMode: false,
    useSignalR: true,
    useView: true,
    usePopup: true,
    useOfflineMode: false,
    useCompactMode: false,
    useVirtualScroll: false,
    useRefreshAtLanguageChange: true,
  });
