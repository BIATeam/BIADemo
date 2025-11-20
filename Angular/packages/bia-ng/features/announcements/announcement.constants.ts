import { CrudConfig } from 'packages/bia-ng/shared/public-api';
import {
  Announcement,
  announcementFieldsConfiguration,
  announcementFormLayoutConfiguration,
} from './model/announcement';

// TODO after creation of CRUD Announcement : adapt the global configuration
export const announcementCRUDConfiguration: CrudConfig<Announcement> =
  new CrudConfig({
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'announcements',
    fieldsConfig: announcementFieldsConfiguration,
    formLayoutConfig: announcementFormLayoutConfiguration,
    displayHistorical: true,
    useCalcMode: false,
    useSignalR: false,
    useView: false,
    usePopup: true,
    useSplit: false,
    useOfflineMode: false,
    useCompactMode: false,
    useVirtualScroll: false,
    useRefreshAtLanguageChange: true,
    // IMPORTANT: this key should be unique in all the application.
    // storeKey: 'feature-' + featureName,
    // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
    // tableStateKey: featureName + 'Grid',
  });
