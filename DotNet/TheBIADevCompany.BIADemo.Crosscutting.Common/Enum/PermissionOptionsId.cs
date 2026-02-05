// <copyright file="PermissionOptionsId.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum
{
    /// <summary>
    /// Permission identifiers for BIADemo project OPTIONS (ordinales for JWT compaction).
    /// IMPORTANT: Keep enum member order in sync with PermissionOptions enum in Angular/src/app/shared/permission-options.ts
    /// When adding new permissions:
    /// 1. Add to both PermissionOptions.ts (Angular) and this enum (C#) in the SAME ORDER
    /// 2. DO NOT specify explicit values - order is implicit (0, 1, 2, ...)
    /// 3. These permissions start at ID 1000 (Options range: 1000-1999)
    /// 4. If out of sync, fallback to string claims ensures zero auth breakage, but watch logs for warnings
    /// </summary>
    public enum PermissionOptionsId
    {
        /// <summary>
        /// Site Options.
        /// </summary>
        Site_Options = 1000,

        /// <summary>
        /// Country Options.
        /// </summary>
        Country_Options,

        /// <summary>
        /// Airport Options.
        /// </summary>
        Airport_Options,

        /// <summary>
        /// Plane Options.
        /// </summary>
        Plane_Options,

        /// <summary>
        /// Plane Type Options.
        /// </summary>
        PlaneType_Options,

        /// <summary>
        /// Aircraft Maintenance Company Options.
        /// </summary>
        AircraftMaintenanceCompany_Options,

        /// <summary>
        /// Announcement Type Options.
        /// </summary>
        AnnouncementType_Options,

        /// <summary>
        /// Part Options.
        /// </summary>
        Part_Options,
    }
}
