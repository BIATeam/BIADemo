import { CrudConfig } from 'packages/bia-ng/shared/public-api';
import {
  AircraftMaintenanceCompany,
  aircraftMaintenanceCompanyFieldsConfiguration,
  aircraftMaintenanceCompanyFormLayoutConfiguration,
} from './model/aircraft-maintenance-company';

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the global configuration
export const aircraftMaintenanceCompanyCRUDConfiguration: CrudConfig<AircraftMaintenanceCompany> =
  new CrudConfig({
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'aircraft-maintenance-companies',
    fieldsConfig: aircraftMaintenanceCompanyFieldsConfiguration,
    formLayoutConfig: aircraftMaintenanceCompanyFormLayoutConfiguration,
    useCalcMode: false,
    useSignalR: false,
    useView: false,
    usePopup: true,
    useSplit: false,
    useOfflineMode: false,
    useCompactMode: false,
    useVirtualScroll: false,
    // IMPORTANT: this key should be unique in all the application.
    // storeKey: 'feature-' + featureName,
    // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
    // tableStateKey: featureName + 'Grid',
  });
