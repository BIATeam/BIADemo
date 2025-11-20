import { CrudConfig } from 'packages/bia-ng/shared/public-api';
import {
  Annoucement,
  annoucementFieldsConfiguration,
  annoucementFormLayoutConfiguration,
} from './model/annoucement';

// TODO after creation of CRUD Annoucement : adapt the global configuration
export const annoucementCRUDConfiguration: CrudConfig<Annoucement> =
  new CrudConfig({
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'annoucements',
    fieldsConfig: annoucementFieldsConfiguration,
    formLayoutConfig: annoucementFormLayoutConfiguration,
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
