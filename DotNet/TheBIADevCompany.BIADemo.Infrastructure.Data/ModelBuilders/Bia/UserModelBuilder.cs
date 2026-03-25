// <copyright file="UserModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public partial class UserModelBuilder : BaseUserModelBuilder
    {
        private static void BiaCreateUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Email).HasMaxLength(256);
#if BIA_USER_CUSTOM_FIELDS_BACK
                entity.Property(u => u.DistinguishedName).IsRequired().HasMaxLength(250);
                entity.Property(u => u.IsEmployee);
                entity.Property(u => u.IsExternal);
                entity.Property(u => u.ExternalCompany).HasMaxLength(50);
                entity.Property(u => u.Company).HasMaxLength(50);
                entity.Property(u => u.Site).HasMaxLength(50);
                entity.Property(u => u.Manager).HasMaxLength(250);
                entity.Property(u => u.Department).HasMaxLength(50);
                entity.Property(u => u.SubDepartment).HasMaxLength(50);
                entity.Property(u => u.Office).HasMaxLength(20);
                entity.Property(u => u.Country).HasMaxLength(10);
#endif
            });
        }

        private static void BiaCreateTeamTypeModelData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.Site, Name = "Site" });
        }

        private static void BiaCreateTeamTypeRoleModelData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(p => p.TeamTypes)
                .WithMany(r => r.Roles)
                .UsingEntity(rt =>
                {
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Site, RolesId = (int)RoleId.SiteAdmin });
                });
        }

        private static void BiaCreateRoleModelData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.SiteAdmin, Code = "Site_Admin", Label = "Site administrator" });
        }
    }
}