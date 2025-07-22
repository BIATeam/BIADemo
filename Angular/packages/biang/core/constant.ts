/* eslint-disable @typescript-eslint/naming-convention */
export const ROUTE_DATA_BREADCRUMB = 'breadcrumb';
export const ROUTE_DATA_CAN_NAVIGATE = 'canNavigate';
export const ROUTE_DATA_NO_MARGIN = 'noMargin';
export const ROUTE_DATA_NO_PADDING = 'noPadding';

export const THEME_LIGHT = 'light';
export const THEME_DARK = 'dark';

export const TABLE_FILTER_GLOBAL = 'global|';

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

export enum TeamTypeId {
  All = 0,
  Root = 1,
  Site = 2,
}
