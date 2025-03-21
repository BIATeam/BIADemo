import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
// BIAToolKit - Begin AncestorTeam AircraftMaintenanceCompany
import { TeamTypeId } from 'src/app/shared/constants';
// BIAToolKit - End AncestorTeam AircraftMaintenanceCompany
import {
  MaintenanceTeam,
  maintenanceTeamFieldsConfiguration,
  maintenanceTeamFormLayoutConfiguration,
} from './model/maintenance-team';

// TODO after creation of CRUD Team MaintenanceTeam : adapt the global configuration
export const maintenanceTeamCRUDConfiguration: CrudConfig<MaintenanceTeam> =
  new CrudConfig({
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'maintenance-teams',
    fieldsConfig: maintenanceTeamFieldsConfiguration,
    formLayoutConfig: maintenanceTeamFormLayoutConfiguration,
    useCalcMode: false,
    useSignalR: false,
    useView: true,
    // BIAToolKit - Begin AncestorTeam AircraftMaintenanceCompany
    useViewTeamWithTypeId: TeamTypeId.AircraftMaintenanceCompany, // use to filter view by teams => should know the type of team
    // BIAToolKit - End AncestorTeam AircraftMaintenanceCompany
    usePopup: true,
    useOfflineMode: false,
    // IMPORTANT: this key should be unique in all the application.
    // storeKey: 'feature-' + featureName,
    // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
    // tableStateKey: featureName + 'Grid',
  });
