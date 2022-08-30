import { TeamTypeId } from "src/app/shared/constants";

// IMPORTANT: this key should be unique in all the application. Use the path of the feature.
export const storeKey: string = 'feature-planes';
// IMPORTANT: this is the key used for the view management it should be unique in all the application.
export const tableStateKey: string = 'planesGrid';

export const useCalcMode = false;
export const useSignalR = false;
export const useView = true;
export const useViewTeamWithTypeId = TeamTypeId.Site;
export const usePopup = true;
export const useOfflineMode = true;