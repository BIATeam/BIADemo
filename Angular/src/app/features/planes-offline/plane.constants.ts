import { TeamTypeId } from "src/app/shared/constants";


export const featureName: string = 'planes';
// IMPORTANT: this key should be unique in all the application. Use the path of the feature.
export const storeKey: string = 'feature-' + featureName;
// IMPORTANT: this is the key used for the view management it should be unique in all the application.
export const tableStateKey: string = featureName + 'Grid';

// TODO after CRUD creation : adapt the default values:
export const useCalcMode = false;
export const useSignalR = false;
export const useView = false;
export const useViewTeamWithTypeId = TeamTypeId.Site; // use to filter view by teams => should know the type of team
export const usePopup = true;
export const useOfflineMode = true;