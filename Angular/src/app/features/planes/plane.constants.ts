import { TeamTypeId } from "src/app/shared/constants";

// IMPORTANT: this key should be unique in all the application.
export const featureName: string = 'planes';

// TODO after CRUD creation : adapt the default values:
export const useCalcMode = false;
export const useSignalR = false;
export const useView = false;
export const useViewTeamWithTypeId = TeamTypeId.Site; // use to filter view by teams => should know the type of team
export const usePopup = true;
export const useOfflineMode = false;