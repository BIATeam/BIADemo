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
    public abstract class BaseAuditModelBuilder
    {
        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateAuditModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().Property(u => u.Table).IsRequired().HasMaxLength(50);
        }

        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <typeparam name="TAuditUser">The audit user entity.</typeparam>
        /// <typeparam name="TUser">The user entity audited.</typeparam>
        protected virtual void CreateUserAuditModel<TAuditUser, TUser>(ModelBuilder modelBuilder)
            where TAuditUser : BaseUserAudit<TUser>
            where TUser : BaseEntityUser
        {
            modelBuilder.Entity<TAuditUser>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TAuditUser>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TAuditUser>().Property(u => u.Login).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<TAuditUser>().Property(u => u.Domain).IsRequired().HasDefaultValue("--");
        }
    }
}