import { TeamTypeId } from "src/app/shared/constants";

// IMPORTANT: this key should be unique in all the application. Use the path of the feature.
export const storeKey: string = 'feature-maintenance-teams';

export const useCalcMode = false;
export const useSignalR = false;
export const useView = true;
export const useViewTeams = [TeamTypeId.AircraftMaintenanceCompany];
export const usePopup = true;