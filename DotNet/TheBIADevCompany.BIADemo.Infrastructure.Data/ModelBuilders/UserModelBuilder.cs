// <copyright file="UserModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public partial class UserModelBuilder : BaseUserModelBuilder
    {
        /// <inheritdoc />
        public override void CreateModel(ModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateUserModel(ModelBuilder modelBuilder)
        {
            base.CreateUserModel(modelBuilder);
            BiaCreateUserModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateUserModelData(ModelBuilder modelBuilder)
        {
            base.CreateUserModelData(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateRoleModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateRoleModelData(modelBuilder);

            // Begin BIADemo
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.SiteAdmin, Code = "Site_Admin", Label = "Site administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.SiteAdmin, Code = "Site_Admin", Label = "Airline administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Pilot, Code = "Pilot", Label = "Pilot" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Supervisor, Code = "Supervisor", Label = "Supervisor" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Expert, Code = "Expert", Label = "Expert" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Operator, Code = "Operator", Label = "Operator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.TeamLeader, Code = "Team_Leader", Label = "Team leader" });

            // End BIADemo
            // Begin BIAToolKit Generation Ignore
            // BIAToolKit - Begin Partial RoleModelBuilder AircraftMaintenanceCompany
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.AircraftMaintenanceCompanyAdmin, Code = "AircraftMaintenanceCompany_Admin", Label = "AircraftMaintenanceCompany administrator" });

            // BIAToolKit - End Partial RoleModelBuilder AircraftMaintenanceCompany
            // BIAToolKit - Begin Partial RoleModelBuilder MaintenanceTeam
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.MaintenanceTeamAdmin, Code = "MaintenanceTeam_Admin", Label = "MaintenanceTeam administrator" });

            // BIAToolKit - End Partial RoleModelBuilder MaintenanceTeam
            // End BIAToolKit Generation Ignore

            // BIAToolKit - Begin RoleModelBuilder
            // BIAToolKit - End RoleModelBuilder
        }

        /// <inheritdoc />
        protected override void CreateUserRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateUserRoleModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateUserRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateUserRoleModelData(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateTeamTypeModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateTeamTypeModelData(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeModelData(modelBuilder);

            // Begin BIAToolKit Generation Ignore
            // BIAToolKit - Begin Partial TeamTypeModelBuilder AircraftMaintenanceCompany
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.AircraftMaintenanceCompany, Name = "AircraftMaintenanceCompany" });

            // BIAToolKit - End Partial TeamTypeModelBuilder AircraftMaintenanceCompany
            // BIAToolKit - Begin Partial TeamTypeModelBuilder MaintenanceTeam
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.MaintenanceTeam, Name = "MaintenanceTeam" });

            // BIAToolKit - End Partial TeamTypeModelBuilder MaintenanceTeam
            // End BIAToolKit Generation Ignore

            // BIAToolKit - Begin TeamTypeModelBuilder
            // BIAToolKit - End TeamTypeModelBuilder
        }

        /// <inheritdoc />
        protected override void CreateTeamTypeRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeRoleModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateTeamTypeRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeRoleModelData(modelBuilder);

            modelBuilder.Entity<Role>()
                .HasMany(p => p.TeamTypes)
                .WithMany(r => r.Roles)
                .UsingEntity(rt =>
                {
                    // Begin BIADemo
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Site, RolesId = (int)RoleId.SiteAdmin });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Site, RolesId = (int)RoleId.Pilot });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.Supervisor });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.Expert });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.Operator });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.Expert });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.TeamLeader });

                    // End BIADemo
                    // Begin BIAToolKit Generation Ignore
                    // BIAToolKit - Begin Partial TeamTypeRoleModelBuilder AircraftMaintenanceCompany
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.AircraftMaintenanceCompanyAdmin });

                    // BIAToolKit - End Partial TeamTypeRoleModelBuilder AircraftMaintenanceCompany
                    // BIAToolKit - Begin Partial TeamTypeRoleModelBuilder MaintenanceTeam
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.MaintenanceTeamAdmin });

                    // BIAToolKit - End Partial TeamTypeRoleModelBuilder MaintenanceTeam
                    // End BIAToolKit Generation Ignore

                    // BIAToolKit - Begin TeamTypeRoleModelBuilder
                    // BIAToolKit - End TeamTypeRoleModelBuilder
                });
        }

        /// <inheritdoc />
        protected override void CreateMemberRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateMemberRoleModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateMemberRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateMemberRoleModelData(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateTeamModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateTeamModelData(ModelBuilder modelBuilder)
        {
            base.CreateTeamModelData(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateUserDefaultTeamModel(ModelBuilder modelBuilder)
        {
            base.CreateUserDefaultTeamModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateUserDefaultTeamModelData(ModelBuilder modelBuilder)
        {
            base.CreateUserDefaultTeamModelData(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateMemberModel(ModelBuilder modelBuilder)
        {
            base.CreateMemberModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateMemberModelData(ModelBuilder modelBuilder)
        {
            base.CreateMemberModelData(modelBuilder);
        }
    }
}
