import { Injectable } from '@angular/core';
import { PermissionInfo } from 'packages/bia-ng/models/permission-info';
import { initializePermissionRegistry } from '../permission-registry';

/**
 * Service to manage dynamic permissions loaded from backend.
 * Provides a single source of truth for permission names and IDs.
 * Also initializes the global Permission constant for easy usage.
 */
@Injectable({
  providedIn: 'root',
})
export class DynamicPermissionService {
  private nameToId = new Map<string, number>();
  private idToName = new Map<number, string>();
  private categoryMap = new Map<string, string>();

  /**
   * Initialize the service with permissions from AppSettings
   * This also populates the global Permission constant
   */
  initialize(permissions: PermissionInfo[] | undefined): void {
    if (!permissions) {
      console.warn(
        '[DynamicPermissionService] No permissions provided in AppSettings'
      );
      return;
    }

    this.nameToId.clear();
    this.idToName.clear();
    this.categoryMap.clear();

    permissions.forEach(p => {
      this.nameToId.set(p.name, p.permissionId);
      this.idToName.set(p.permissionId, p.name);
      this.categoryMap.set(p.name, p.category);
    });

    // Initialize the global Permission constant
    initializePermissionRegistry(permissions);

    console.log(
      `[DynamicPermissionService] Initialized with ${permissions.length} permissions`
    );
  }

  /**
   * Get permission ID by name
   */
  getPermissionId(name: string): number | undefined {
    return this.nameToId.get(name);
  }

  /**
   * Get permission name by ID
   */
  getPermissionName(id: number): string | undefined {
    return this.idToName.get(id);
  }

  /**
   * Get permission category (BiaPermission, Permission, OptionPermission)
   */
  getPermissionCategory(name: string): string | undefined {
    return this.categoryMap.get(name);
  }

  /**
   * Check if a permission exists
   */
  hasPermission(name: string): boolean {
    return this.nameToId.has(name);
  }

  /**
   * Get all permission names
   */
  getAllPermissionNames(): string[] {
    return Array.from(this.nameToId.keys());
  }

  /**
   * Get all permissions by category
   */
  getPermissionsByCategory(
    category: 'BiaPermission' | 'Permission' | 'OptionPermission'
  ): string[] {
    return this.getAllPermissionNames().filter(
      name => this.categoryMap.get(name) === category
    );
  }
}
