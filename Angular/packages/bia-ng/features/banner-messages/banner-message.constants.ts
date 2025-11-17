import { CrudConfig } from 'packages/bia-ng/shared/public-api';
import {
  BannerMessage,
  bannerMessageFieldsConfiguration,
  bannerMessageFormLayoutConfiguration,
} from './model/banner-message';

// TODO after creation of CRUD BannerMessage : adapt the global configuration
export const bannerMessageCRUDConfiguration: CrudConfig<BannerMessage> =
  new CrudConfig({
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'banner-messages',
    fieldsConfig: bannerMessageFieldsConfiguration,
    formLayoutConfig: bannerMessageFormLayoutConfiguration,
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
