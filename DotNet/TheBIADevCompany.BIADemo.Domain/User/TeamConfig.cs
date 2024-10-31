// <copyright file="TeamConfig.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Domain.User
{
    using System.Collections.Immutable;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Helpers;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    // Begin BIADemo

    // End BIADemo

    /// <summary>
    /// Team prefixe.
    /// </summary>
    public static class TeamConfig
    {
        /// <summary>
        /// the private mapping.
        /// </summary>
        public static readonly ImmutableList<BiaTeamConfig<Team>> Config = new ImmutableListBuilder<BiaTeamConfig<Team>>()
        {
            new BiaTeamConfig<Team>()
            {
                TeamTypeId = (int)TeamTypeId.Site,
                RightPrefix = "Site",
                AdminRoleIds = new int[] { (int)RoleId.SiteAdmin },
            },

            // BIAToolKit - Begin TeamConfig
            // BIAToolKit - End TeamConfig

            // Begin BIADemo
            new BiaTeamConfig<Team>()
            {
                TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                RightPrefix = "AircraftMaintenanceCompany",
                AdminRoleIds = new int[] { (int)RoleId.Supervisor },
                Children = new ImmutableListBuilder<BiaTeamChildrenConfig<Team>>
                {
                    new BiaTeamChildrenConfig<Team>
                    {
                        TeamTypeId = (int)TeamTypeId.MaintenanceTeam,
                        GetChilds = team => (team as AircraftMaintenanceCompany).MaintenanceTeams,
                    },
                }.ToImmutable(),
            },

            // BIAToolKit - Begin Partial TeamConfig MaintenanceTeam
            new BiaTeamConfig<Team>()
            {
                TeamTypeId = (int)TeamTypeId.MaintenanceTeam,
                RightPrefix = "MaintenanceTeam",
                AdminRoleIds = new int[] { (int)RoleId.MaintenanceTeamAdmin },

                // BIAToolKit - Begin Nested Parent AircraftMaintenanceCompany
                Parents = new ImmutableListBuilder<BiaTeamParentConfig<Team>>
                {
                    new BiaTeamParentConfig<Team>
                    {
                        TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                        GetParent = team => (team as MaintenanceTeam).AircraftMaintenanceCompany,
                    },
                }
                .ToImmutable(),

                // BIAToolKit - End Nested Parent AircraftMaintenanceCompany
            },

            // BIAToolKit - End Partial TeamConfig MaintenanceTeam

            // End BIADemo
        }.ToImmutable();
    }
}
