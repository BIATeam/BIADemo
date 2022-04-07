import { TeamTypeId } from "src/app/shared/constants";

// IMPORTANT: this key should be unique in all the application. Use the path of the feature.
export const storeKey: string = 'features-planes-view';

export const useCalcMode = false;
export const useSignalR = false;
export const useView = true;
export const useViewTeamId = TeamTypeId.Site;
export const usePopup = true;