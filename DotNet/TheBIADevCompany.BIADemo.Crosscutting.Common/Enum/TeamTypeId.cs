// <copyright file="TeamTypeId.cs" company="TheBIADevCompany">
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
        /// Value for site.
        /// </summary>
        Site = 1,

        // Begin BIADemo

        /// <summary>
        /// Value for Aircraft Maintenance Company.
        /// </summary>
        AircraftMaintenanceCompany = 2,

        /// <summary>
        /// Value for Maintenance Team.
        /// </summary>
        MaintenanceTeam = 3,

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