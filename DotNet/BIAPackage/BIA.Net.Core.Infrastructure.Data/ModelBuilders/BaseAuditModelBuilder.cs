// <copyright file="BaseAuditModelBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public class BaseAuditModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public virtual void CreateModel(ModelBuilder modelBuilder)
        {
            this.CreateUserAuditModel(modelBuilder);
            this.CreateAuditModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().HasKey(u => new { u.Id });
            modelBuilder.Entity<AuditLog>().Property(u => u.Table).IsRequired().HasMaxLength(50);
        }

        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateUserAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAudit>().HasKey(u => new { u.Id });
            modelBuilder.Entity<UserAudit>().Property(p => p.EntityId).IsRequired();
            modelBuilder.Entity<UserAudit>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserAudit>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserAudit>().Property(u => u.Login).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<UserAudit>().Property(u => u.Domain).IsRequired().HasDefaultValue("--");
        }
    }
}