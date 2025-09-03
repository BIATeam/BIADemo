// <copyright file="TeamConfig.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Domain.User
{
    using System.Collections.Immutable;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// Team prefixe.
    /// </summary>
    public static class TeamConfig
    {
        /// <summary>
        /// the private mapping.
        /// </summary>
        public static readonly ImmutableList<BiaTeamConfig<BaseEntityTeam>> Config = new ImmutableListBuilder<BiaTeamConfig<BaseEntityTeam>>()
        {
            new BiaTeamConfig<BaseEntityTeam>()
            {
                TeamTypeId = (int)TeamTypeId.Site,
                RightPrefix = "Site",
                AdminRoleIds = new int[] { (int)RoleId.SiteAdmin },
                RoleMode = BIA.Net.Core.Common.Enum.RoleMode.AllRoles,
                DisplayInHeader = true,
                Label = "site.headerLabel",
            },

            // BIAToolKit - Begin TeamConfig
            // BIAToolKit - End TeamConfig

            // Begin BIAToolKit Generation Ignore
            // BIAToolKit - Begin Partial TeamConfig AircraftMaintenanceCompany
            new BiaTeamConfig<BaseEntityTeam>()
            {
                TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                RightPrefix = "AircraftMaintenanceCompany",
                AdminRoleIds = [

                    // Begin BIADemo
                    (int)RoleId.Supervisor,

                    // End BIADemo
                    (int)RoleId.AircraftMaintenanceCompanyAdmin
                    ],
                Children = new ImmutableListBuilder<BiaTeamChildrenConfig<BaseEntityTeam>>
                {
                // BIAToolKit - Begin TeamConfigAircraftMaintenanceCompanyChildren
                // BIAToolKit - Begin Partial TeamConfigAircraftMaintenanceCompanyChildren MaintenanceTeam
                    new BiaTeamChildrenConfig<BaseEntityTeam>
                    {
                        TeamTypeId = (int)TeamTypeId.MaintenanceTeam,
                        GetChilds = team => (team as Maintenance.Entities.AircraftMaintenanceCompany).MaintenanceTeams,
                    },

                // BIAToolKit - End Partial TeamConfigAircraftMaintenanceCompanyChildren MaintenanceTeam
                // BIAToolKit - End TeamConfigAircraftMaintenanceCompanyChildren
                }.ToImmutable(),
                Label = "aircraftMaintenanceCompany.headerLabel",

                // Begin BIADemo
                RoleMode = BIA.Net.Core.Common.Enum.RoleMode.MultiRoles,
                DisplayInHeader = true,
                DisplayOne = true,

                // End BIADemo
            },

            // BIAToolKit - End Partial TeamConfig AircraftMaintenanceCompany
            // BIAToolKit - Begin Partial TeamConfig MaintenanceTeam
            new BiaTeamConfig<BaseEntityTeam>()
            {
                TeamTypeId = (int)TeamTypeId.MaintenanceTeam,
                RightPrefix = "MaintenanceTeam",
                AdminRoleIds = [
                    (int)RoleId.MaintenanceTeamAdmin
                    ],
                Children = new ImmutableListBuilder<BiaTeamChildrenConfig<BaseEntityTeam>>
                {
                // BIAToolKit - Begin TeamConfigMaintenanceTeamChildren
                // BIAToolKit - End TeamConfigMaintenanceTeamChildren
                }.ToImmutable(),
                Parents = new ImmutableListBuilder<BiaTeamParentConfig<BaseEntityTeam>>
                {
                    new BiaTeamParentConfig<BaseEntityTeam>
                    {
                        TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany,
                        GetParent = team => (team as Maintenance.Entities.MaintenanceTeam).AircraftMaintenanceCompany,
                    },
                }
                .ToImmutable(),
                Label = "maintenanceTeam.headerLabel",

                // Begin BIADemo
                TeamAutomaticSelectionMode = BIA.Net.Core.Common.Enum.TeamSelectionMode.None,
                RoleMode = BIA.Net.Core.Common.Enum.RoleMode.AllRoles,
                DisplayInHeader = true,
                DisplayAlways = true,
                DisplayLabel = true,
                TeamSelectionCanBeEmpty = true,

                // End BIADemo
            },

            // BIAToolKit - End Partial TeamConfig MaintenanceTeam
            // End BIAToolKit Generation Ignore
        }.ToImmutable();
    }
}
