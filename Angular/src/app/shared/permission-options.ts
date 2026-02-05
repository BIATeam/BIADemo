/**
 * Permission Options enum (values = permission strings for auth checks)
 * IMPORTANT: Keep enum member order in sync with PermissionOptionsId enum in DotNet/TheBIADevCompany.BIADemo.Crosscutting.Common/Enum/PermissionOptionsId.cs
 * When adding new permissions:
 * 1. Add to both this enum (Angular) and PermissionOptionsId.cs (C#) in the SAME ORDER
 * 2. These permissions use ID offset 1000 in the JWT
 * 3. If out of sync, fallback to string claims ensures zero auth breakage
 */
/* eslint-disable @typescript-eslint/naming-convention */
export enum PermissionOptions {
  Site_Options = 'Site_Options',
  Country_Options = 'Country_Options',
  Airport_Options = 'Airport_Options',
  Plane_Options = 'Plane_Options',
  PlaneType_Options = 'PlaneType_Options',
  AircraftMaintenanceCompany_Options = 'AircraftMaintenanceCompany_Options',
  AnnouncementType_Options = 'AnnouncementType_Options',
  Part_Options = 'Part_Options',
}
