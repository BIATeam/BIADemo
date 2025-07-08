// <copyright file="DistCacheModelBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.DistCache.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class used to update the model builder for distributed cache domain.
    /// </summary>
    public static class DistCacheModelBuilder
    {
        /// <summary>
        /// Create the model for distributed cache.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateDistCacheModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DistCache>(entity =>
            {
                entity.HasIndex(e => e.ExpiresAtTime)
                    .HasDatabaseName("Index_ExpiresAtTime");

                entity.Property(e => e.Id).HasMaxLength(449);

                entity.Property(e => e.Value).IsRequired();
            });
        }
    }
}