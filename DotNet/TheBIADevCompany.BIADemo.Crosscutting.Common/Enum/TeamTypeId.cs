// <copyright file="TeamTypeId.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
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

        // Begin BIADemo
#pragma warning disable S1135 // Complete the task associated to this 'TODO' comment.
        /// <summary>
        /// Value for Aircraft Maintenance Company.
        /// </summary>
        AircraftMaintenanceCompany = 3,

        // BIAToolKit - Begin Partial TeamTypeId MaintenanceTeam

        /// <summary>
        /// Value for Maintenance Team.
        /// </summary>
        // TODO after creation of team MaintenanceTeam : adapt the enum value
        MaintenanceTeam = 4,

        // BIAToolKit - End Partial TeamTypeId MaintenanceTeam
#pragma warning restore S1135 // Complete the task associated to this 'TODO' comment.

        // End BIADemo

        // BIAToolKit - Begin TeamTypeId
        // BIAToolKit - End TeamTypeId
    }
}