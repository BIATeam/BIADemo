import { CrudConfig } from 'bia-ng/shared';
import {
  MaintenanceContract,
  maintenanceContractFieldsConfiguration,
} from './model/maintenance-contract';

// TODO after creation of CRUD MaintenanceContract : adapt the global configuration
export const maintenanceContractCRUDConfiguration: CrudConfig<MaintenanceContract> =
  new CrudConfig({
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'maintenanceContracts',
    fieldsConfig: maintenanceContractFieldsConfiguration,
    useCalcMode: true,
    useSignalR: false,
    useView: true,
    usePopup: true,
    useOfflineMode: false,
    useCompactMode: false,
    useVirtualScroll: false,
    // IMPORTANT: this key should be unique in all the application.
    // storeKey: 'feature-' + featureName,
    // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
    // tableStateKey: featureName + 'Grid',
  });
