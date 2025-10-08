// <copyright file="AuditModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    // End BIADemo

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public class AuditModelBuilder : BaseAuditModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public void CreateModel(ModelBuilder modelBuilder)
        {
            this.CreateAuditModel(modelBuilder);
            this.CreateUserAuditModel<UserAudit, User>(modelBuilder);

            // Add here the project specific audit model creation.
            // Begin BIADemo
            modelBuilder.Entity<PlaneAudit>().Property(p => p.EntityId).IsRequired();
            modelBuilder.Entity<EngineAudit>().Property(p => p.EntityId).IsRequired();
            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.EntityId).IsRequired();

            // End BIADemo
        }
    }
}