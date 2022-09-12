import { CrudConfig } from "src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config";
import { TeamTypeId } from "src/app/shared/constants";
import { PlaneFieldsConfiguration } from "./model/plane";

// IMPORTANT: this key should be unique in all the application.
export const featureName: string = 'planes-specific';

// TODO after CRUD creation : adapt the default values:
export const useCalcMode = false;
export const useSignalR = false;
export const useView = false;
export const useViewTeamWithTypeId = TeamTypeId.Site; // use to filter view by teams => should know the type of team
export const usePopup = true;
export const useOfflineMode = false;

// TODO after CRUD creation : adapt the global configuration
export const PlaneCRUDConfiguration : CrudConfig = new CrudConfig(
    {
      featureName: featureName,
      fieldsConfig: PlaneFieldsConfiguration,
      useCalcMode: useCalcMode,
      useSignalR: useSignalR,
      useView: useView,
      useViewTeamWithTypeId: useViewTeamWithTypeId,
      usePopup: usePopup,
      useOfflineMode: useOfflineMode,
      // IMPORTANT: this key should be unique in all the application.
      // storeKey: 'feature-' + featureName,
      // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
      tableStateKey: 'planesGrid',
    }
  )