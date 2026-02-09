import { BiaTeamTypeId } from 'packages/bia-ng/models/enum/public-api';

/* eslint-disable @typescript-eslint/naming-convention */
export const APP_SUPPORTED_TRANSLATIONS = [
  'es-ES',
  'fr-FR',
  'en-GB',
  'es-MX',
  'en-US',
];
// English is default value (see translation.defaultLanguage in i18n) so we do not add it manualy.
export const APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY = [1];
export const DEFAULT_PAGE_SIZE = 10;

export const ROUTE_DATA_BREADCRUMB = 'breadcrumb';
export const ROUTE_DATA_CAN_NAVIGATE = 'canNavigate';
export const ROUTE_DATA_NO_MARGIN = 'noMargin';
export const ROUTE_DATA_NO_PADDING = 'noPadding';

export const THEME_LIGHT = 'light';
export const THEME_DARK = 'dark';

export const TABLE_FILTER_GLOBAL = 'global|';
export const DEFAULT_POPUP_MINWIDTH = '60vw';

export enum ViewType {
  System = 0,
  Team = 1,
  User = 2,
}

export enum RoleMode {
  /// <summary>
  ///  All possible roles are selected
  /// </summary>
  AllRoles = 1,

  /// <summary>
  /// Only one role is selectable
  /// </summary>
  SingleRole = 2,

  /// <summary>
  /// Multi select Role
  /// </summary>
  MultiRoles = 3,
}

enum AppTeamTypeId {
  // BIAToolKit - Begin TeamTypeIdConstants
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial TeamTypeIdConstants AircraftMaintenanceCompany
  AircraftMaintenanceCompany = 3,
  // BIAToolKit - End Partial TeamTypeIdConstants AircraftMaintenanceCompany
  // BIAToolKit - Begin Partial TeamTypeIdConstants MaintenanceTeam
  MaintenanceTeam = 4,
  // BIAToolKit - End Partial TeamTypeIdConstants MaintenanceTeam
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End TeamTypeIdConstants
}

export type TeamTypeId = AppTeamTypeId | BiaTeamTypeId;
export const TeamTypeId = { ...AppTeamTypeId, ...BiaTeamTypeId };

const TeamTypeRightPrefix: { key: TeamTypeId; value: string }[] = [
  { key: TeamTypeId.Site, value: 'Site' },
  // BIAToolKit - Begin TeamTypeRightPrefixConstants
  // Begin BIAToolKit Generation Ignore
  // BIAToolKit - Begin Partial TeamTypeRightPrefixConstants AircraftMaintenanceCompany
  {
    key: TeamTypeId.AircraftMaintenanceCompany,
    value: 'AircraftMaintenanceCompany',
  },
  // BIAToolKit - End Partial TeamTypeRightPrefixConstants AircraftMaintenanceCompany
  // BIAToolKit - Begin Partial TeamTypeRightPrefixConstants MaintenanceTeam
  { key: TeamTypeId.MaintenanceTeam, value: 'MaintenanceTeam' },
  // BIAToolKit - End Partial TeamTypeRightPrefixConstants MaintenanceTeam
  // End BIAToolKit Generation Ignore
  // BIAToolKit - End TeamTypeRightPrefixConstants
];
export { TeamTypeRightPrefix };
