// <copyright file="UserExtendedModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders.Bia
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public class UserExtendedModelBuilder : UserModelBuilder
    {
        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public override void CreateUserModel(ModelBuilder modelBuilder)
        {
            base.CreateUserModel(modelBuilder);
            modelBuilder.Entity<UserExtended>(entity =>
            {
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
            });
        }
    }
}