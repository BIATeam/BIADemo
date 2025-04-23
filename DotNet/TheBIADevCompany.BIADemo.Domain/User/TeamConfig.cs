// <copyright file="TeamConfig.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Domain.User
{
    using System.Collections.Immutable;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Helpers;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

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

            new BiaTeamConfig<Team>()
            {
                TeamTypeId = (int)TeamTypeId.MaintenanceTeam,
                RightPrefix = "MaintenanceTeam",
                AdminRoleIds = new int[] { (int)RoleId.MaintenanceTeamAdmin },

                Parents = new ImmutableListBuilder<BiaTeamParentConfig<Team>>
                {
                    new BiaTeamParentConfig<Team>
                    {
                        TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                        GetParent = team => (team as MaintenanceTeam).AircraftMaintenanceCompany,
                    },
                }
                .ToImmutable(),
            },

            // End BIADemo
        }.ToImmutable();
    }
}
