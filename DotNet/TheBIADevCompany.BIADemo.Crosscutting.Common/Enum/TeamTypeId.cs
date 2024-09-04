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

        /// <summary>
        /// Value for Aircraft Maintenance Company.
        /// </summary>
        AircraftMaintenanceCompany = 3,

        /// <summary>
        /// Value for Maintenance Team.
        /// </summary>
        MaintenanceTeam = 4,

        // End BIADemo
    }
}