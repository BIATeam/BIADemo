/**
 * Global Permission Registry
 *
 * This object contains ALL application permissions (BiaPermission, Permission, OptionPermission)
 * loaded dynamically from the backend via AppSettings.
 *
 * USAGE:
 * ```typescript
 * import { Permission } from 'packages/bia-ng/core/permission-registry';
 *
 * // Use in guards, directives, services:
 * if (hasPermission(Permission.Site_Create)) { }
 * if (hasPermission(Permission.User_Add)) { }
 * if (hasPermission(Permission.Site_Options)) { }
 * ```
 *
 * BENEFITS:
 * - ✅ Single source of truth (backend)
 * - ✅ Full TypeScript autocompletion
 * - ✅ Type safety at compile time
 * - ✅ Zero desynchronization risk
 * - ✅ Replaces 3 separate enums with 1 unified constant
 *
 * INITIALIZATION:
 * Automatically initialized by AppComponent when AppSettings are loaded.
 */

/**
 * Permission constant object - populated dynamically from backend
 * All permissions are represented as string keys with string values
 */
export const Permission: Record<string, string> = {};

/**
 * Type representing all available permission keys
 * Updated dynamically when permissions are loaded
 */
export type PermissionKey = keyof typeof Permission;

/**
 * Initialize the Permission registry from backend permissions
 * Called automatically by the app initialization process
 *
 * @internal - Do not call manually, this is handled by the framework
 */
export function initializePermissionRegistry(
  permissions: Array<{ name: string; permissionId: number; category: string }>
): void {
  // Clear existing permissions
  Object.keys(Permission).forEach(key => delete Permission[key]);

  // Populate with backend permissions
  permissions.forEach(p => {
    Permission[p.name] = p.name;
  });

  // Make the object readonly to prevent accidental modifications
  Object.freeze(Permission);

  console.log(
    `[Permission Registry] Initialized with ${permissions.length} permissions from backend`
  );
}

/**
 * Check if a permission exists in the registry
 */
export function isValidPermission(permissionName: string): boolean {
  return permissionName in Permission;
}

/**
 * Get all permission names (for debugging/admin purposes)
 */
export function getAllPermissions(): string[] {
  return Object.keys(Permission);
}
