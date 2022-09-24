export const APP_SUPPORTED_TRANSLATIONS = ['es-ES', 'fr-FR', 'en-GB', 'es-MX', 'en-US'];
// English is default value (see translation.defaultLanguage in i18n) so we do not add it manualy.
export const APP_TANSLATION_IDS_TO_NOT_ADD_MANUALY = [1];
export const DEFAULT_PAGE_SIZE = 10;

export const ROUTE_DATA_BREADCRUMB = 'breadcrumb';
export const ROUTE_DATA_CAN_NAVIGATE = 'canNavigate';
export const ROUTE_DATA_NO_MARGIN = 'noMargin';

export const THEME_LIGHT = 'light';
export const THEME_DARK = 'dark';

export const TABLE_FILTER_GLOBAL = 'global|';

export enum ViewType {
  System = 0,
  Team = 1,
  User = 2
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

export enum TeamTypeId {
  All = 0,
  Root = 1,
  Site = 2,
  // Begin BIADemo  
  AircraftMaintenanceCompany = 3,
  MaintenanceTeam = 4,
  // End BIADemo
}

let TeamTypeRightPrefixe :{ key: TeamTypeId; value: string; }[] = [ 
  {key: TeamTypeId.Site, value: "Site"},
// Begin BIADemo
  {key: TeamTypeId.AircraftMaintenanceCompany, value: "AircraftMaintenanceCompany"},
  {key: TeamTypeId.MaintenanceTeam, value: "MaintenanceTeam"},
// End BIADemo
];
export {TeamTypeRightPrefixe};