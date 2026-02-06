/**
 * Permission information received from backend
 */
export interface PermissionInfo {
  /** Permission name (enum member name) */
  name: string;

  /** Numeric permission ID */
  permissionId: number;

  /** Category: 'BiaPermission', 'Permission', or 'OptionPermission' */
  category: string;
}
