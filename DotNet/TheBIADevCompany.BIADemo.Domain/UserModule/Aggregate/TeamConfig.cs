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
        // Begin BIADemo
        public static readonly BIATeamChildrenConfig<TeamTypeId, Team> ChildMaintenanceTeam = new BIATeamChildrenConfig<TeamTypeId, Team> { TypeId = TeamTypeId.MaintenanceTeam, GetChilds = team => (team as AircraftMaintenanceCompany).MaintenanceTeams };
        public static readonly ImmutableList<BIATeamChildrenConfig<TeamTypeId, Team>> AircraftMaintenanceCompanyChildren = new ImmutableListBuilder<BIATeamChildrenConfig<TeamTypeId, Team>> { ChildMaintenanceTeam }.ToImmutable();

        public static readonly BIATeamParentConfig<TeamTypeId, Team> ParentAircraftMaintenanceCompany = new BIATeamParentConfig<TeamTypeId, Team> { TypeId = TeamTypeId.AircraftMaintenanceCompany, GetParent = team => (team as MaintenanceTeam).AircraftMaintenanceCompany };
        public static readonly ImmutableList<BIATeamParentConfig<TeamTypeId, Team>> MaintenanceTeamParents = new ImmutableListBuilder<BIATeamParentConfig<TeamTypeId, Team>> { ParentAircraftMaintenanceCompany }.ToImmutable();

        // End BIADemo

        /// <summary>
        /// the private mapping.
        /// </summary>
        public static readonly ImmutableDictionary<TeamTypeId, BIATeamConfig<TeamTypeId, Team>> Config = new ImmutableDictionaryBuilder<TeamTypeId, BIATeamConfig<TeamTypeId, Team>>()
        {
            { TeamTypeId.Site, new BIATeamConfig<TeamTypeId, Team>() { RightPrefix = "Site" } },

            // Begin BIADemo
            {
                TeamTypeId.AircraftMaintenanceCompany, new BIATeamConfig<TeamTypeId,Team>()
                {
                    RightPrefix = "AircraftMaintenanceCompany",
                    Children = AircraftMaintenanceCompanyChildren,
                }
            },
            {
                TeamTypeId.MaintenanceTeam, new BIATeamConfig<TeamTypeId, Team>()
                {
                    RightPrefix = "MaintenanceTeam",
                    Parents = MaintenanceTeamParents,
                }
            },

            // End BIADemo
        }.ToImmutable();
    }
}
