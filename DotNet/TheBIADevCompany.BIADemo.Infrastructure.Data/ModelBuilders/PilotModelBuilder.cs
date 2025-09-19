// BIADemo only
// <copyright file="PilotModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Class used to update the model builder for pilot domain.
    /// </summary>
    public static class PilotModelBuilder
    {
        /// <summary>
        /// Create the model for projects.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreatePilotModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for pilots.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePilotModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pilot>()
            .Property(e => e.Id)
            /*.HasDefaultValueSql("NEWSEQUENTIALID()")*/;
            modelBuilder.Entity<Pilot>().Property(p => p.IdentificationNumber).IsRequired().HasMaxLength(64);
        }
    }
}