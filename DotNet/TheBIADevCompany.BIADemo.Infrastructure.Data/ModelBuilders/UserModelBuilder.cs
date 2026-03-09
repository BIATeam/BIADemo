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
        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateUserModel(ModelBuilder modelBuilder)
        {
            base.CreateUserModel(modelBuilder);
            BiaCreateUserModel(modelBuilder);
        }

        /// <summary>
        /// Create the model data for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateUserModelData(ModelBuilder modelBuilder)
        {
            base.CreateUserModelData(modelBuilder);
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateRoleModelData(modelBuilder);

            // Begin BIADemo
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
                // DO NOT CHANGE INDENTATION (For BIATemplate)
                // End BIADemo
                BiaCreateRoleModelData(modelBuilder);

                // Begin BIADemo
#pragma warning restore CS0162 // Unreachable code detected
            }

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

        /// <summary>
        /// Create the model for user role.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateUserRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateUserRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model data for user role.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateUserRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateUserRoleModelData(modelBuilder);
        }

        /// <summary>
        /// Create the model for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamTypeModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamTypeModelData(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeModelData(modelBuilder);
            BiaCreateTeamTypeModelData(modelBuilder);

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

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamTypeRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamTypeRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeRoleModelData(modelBuilder);
            BiaCreateTeamTypeRoleModelData(modelBuilder);

            modelBuilder.Entity<Role>()
                .HasMany(p => p.TeamTypes)
                .WithMany(r => r.Roles)
                .UsingEntity(rt =>
                {
                    // Begin BIADemo
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

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateMemberRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateMemberRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model data for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateMemberRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateMemberRoleModelData(modelBuilder);
        }

        /// <summary>
        /// Create the model for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamModel(modelBuilder);
        }

        /// <summary>
        /// Create the model data for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamModelData(ModelBuilder modelBuilder)
        {
            base.CreateTeamModelData(modelBuilder);
        }

        /// <summary>
        /// Create the model for user default teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateUserDefaultTeamModel(ModelBuilder modelBuilder)
        {
            base.CreateUserDefaultTeamModel(modelBuilder);
        }

        /// <summary>
        /// Create the model data for user default teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateUserDefaultTeamModelData(ModelBuilder modelBuilder)
        {
            base.CreateUserDefaultTeamModelData(modelBuilder);
        }

        /// <summary>
        /// Create the model for members.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateMemberModel(ModelBuilder modelBuilder)
        {
            base.CreateMemberModel(modelBuilder);
        }

        /// <summary>
        /// Create the model data for members.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateMemberModelData(ModelBuilder modelBuilder)
        {
            base.CreateMemberModelData(modelBuilder);
        }
    }
}
