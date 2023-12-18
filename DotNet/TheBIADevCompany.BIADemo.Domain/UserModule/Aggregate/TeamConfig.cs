// <copyright file="TeamConfig.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System.Collections.Immutable;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Helpers;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;

    // End BIADemo

    /// <summary>
    /// Team prefixe.
    /// </summary>
    public static class TeamConfig
    {
        /// <summary>
        /// the private mapping.
        /// </summary>
        public static readonly ImmutableList<BIATeamConfig<Team>> Config = new ImmutableListBuilder<BIATeamConfig<Team>>()
        {
            new BIATeamConfig<Team>()
            {
                TeamTypeId = (int)TeamTypeId.Site,
                RightPrefix = "Site",
                AdminRoleIds = new int[] { (int)RoleId.SiteAdmin },
            },

            // Begin BIADemo
            new BIATeamConfig<Team>()
            {
                TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                RightPrefix = "AircraftMaintenanceCompany",
                AdminRoleIds = new int[] { (int)RoleId.Supervisor },
                Children = new ImmutableListBuilder<BIATeamChildrenConfig<Team>>
                {
                    new BIATeamChildrenConfig<Team>
                    {
                        TeamTypeId = (int)TeamTypeId.MaintenanceTeam,
                        GetChilds = team => (team as AircraftMaintenanceCompany).MaintenanceTeams,
                    },
                }.ToImmutable(),
            },
            new BIATeamConfig<Team>()
            {
                TeamTypeId = (int)TeamTypeId.MaintenanceTeam,
                RightPrefix = "MaintenanceTeam",
                AdminRoleIds = new int[] { (int)RoleId.TeamLeader },
                Parents = new ImmutableListBuilder<BIATeamParentConfig<Team>>
                {
                    new BIATeamParentConfig<Team>
                    {
                        TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                        GetParent = team => (team as MaintenanceTeam).AircraftMaintenanceCompany,
                    },
                }.ToImmutable(),
            },

            // End BIADemo
        }.ToImmutable();
    }
}
