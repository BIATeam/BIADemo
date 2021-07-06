// <copyright file="DistCacheModelBuilder.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.DistCacheModule.Aggregate;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class used to update the model builder for plane domain.
    /// </summary>
    public static class DistCacheModelBuilder
    {
        /// <summary>
        /// Create the model for planes.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateDistCacheModel(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DistCache>(entity =>
            {
                entity.HasIndex(e => e.ExpiresAtTime)
                    .HasName("Index_ExpiresAtTime");

                entity.Property(e => e.Id).HasMaxLength(449);

                entity.Property(e => e.Value).IsRequired();
            });
        }
    }
}