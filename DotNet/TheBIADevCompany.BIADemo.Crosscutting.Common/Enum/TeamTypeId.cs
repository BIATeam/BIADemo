﻿// <copyright file="TeamTypeId.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum
{
    using System.Collections.Generic;

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

    /// <summary>
    /// Team prefixe.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name
    public static class TeamTypeRightPrefixe
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// the private mapping.
        /// </summary>
        private static Dictionary<TeamTypeId, string> mapping = new ()
        {
            { TeamTypeId.Site, "Site" },

            // Begin BIADemo
            { TeamTypeId.AircraftMaintenanceCompany, "AircraftMaintenanceCompany" },
            { TeamTypeId.MaintenanceTeam, "MaintenanceTeam" },

            // End BIADemo
        };

        /// <summary>
        /// the mapping.
        /// </summary>
        public static Dictionary<TeamTypeId, string> Mapping
        {
            get { return mapping; }
        }
    }
}