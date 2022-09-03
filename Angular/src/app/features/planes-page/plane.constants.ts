import { TeamTypeId } from "src/app/shared/constants";


export const featureName: string = 'planes';

// TODO after CRUD creation : adapt the default values:
export const useCalcMode = false;
export const useSignalR = false;
export const useView = false;
export const useViewTeamWithTypeId = TeamTypeId.Site; // use to filter view by teams => should know the type of team
export const usePopup = false;
export const useOfflineMode = false;