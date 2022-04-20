import { TeamTypeId } from "src/app/shared/constants";

// IMPORTANT: this key should be unique in all the application. Use the path of the feature.
export const storeKey: string = 'feature-users';

export const useCalcMode = false;
export const useSignalR = false;
export const useView = false;
export const useViewTeamWithTypeId = TeamTypeId.Site;
export const usePopup = true;