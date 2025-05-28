// <copyright file="TeamTypeId.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum
{
    using System.Collections.Immutable;

    /// <summary>
    /// Enum for different kind of team.
    /// </summary>
    public enum TeamTypeId
    {
        /// <summary>
        /// Use in filter to select all team type.
        /// </summary>
        All = 0,

        /// <summary>
        /// Value for site.
        /// </summary>
        Root = 1,

        /// <summary>
        /// Value for site.
        /// </summary>
        Site = 2,

        // Begin BIAToolKit Generation Ignore
#pragma warning disable S1135 // Track uses of "TODO" tags

        // BIAToolKit - Begin Partial TeamTypeId AircraftMaintenanceCompany

        /// <summary>
        /// Value for AircraftMaintenanceCompany.
        /// </summary>
        // TODO after creation of team AircraftMaintenanceCompany : adapt the enum value
        AircraftMaintenanceCompany = 3,

        // BIAToolKit - End Partial TeamTypeId AircraftMaintenanceCompany
        // BIAToolKit - Begin Partial TeamTypeId MaintenanceTeam

        /// <summary>
        /// Value for MaintenanceTeam.
        /// </summary>
        // TODO after creation of team MaintenanceTeam : adapt the enum value
        MaintenanceTeam = 4,

        // BIAToolKit - End Partial TeamTypeId MaintenanceTeam
#pragma warning restore S1135 // Track uses of "TODO" tags

        // End BIAToolKit Generation Ignore

        // BIAToolKit - Begin TeamTypeId
        // BIAToolKit - End TeamTypeId
    }
}