// <copyright file="AuditModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using System.Diagnostics;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

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
        public override void CreateModel(ModelBuilder modelBuilder)
        {
            Debug.Assert(modelBuilder != null, "Line to avoid warning empty method");
            base.CreateModel(modelBuilder);

            // Add here the project specific audit model creation.
            // Begin BIADemo
            modelBuilder.Entity<PlaneAudit>().Property(p => p.EntityId).IsRequired();

            modelBuilder.Entity<EngineAudit>().Property(p => p.EntityId).IsRequired();

            modelBuilder.Entity<PlaneAirportAudit>().Property(p => p.EntityId).IsRequired();

            // End BIADemo
        }
    }
}