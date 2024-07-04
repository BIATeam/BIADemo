// <copyright file="RowVersionBuilder.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System.Linq;
    using BIA.Net.Core.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Build the column RowVersion in each table.
    /// </summary>
    public static class RowVersionBuilder
    {
        /// <summary>
        /// Build the column RowVersion in each table.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateRowVersion(ModelBuilder modelBuilder)
        {
            foreach (var entityType in from entityType in modelBuilder.Model.GetEntityTypes()
                                       where typeof(VersionedTable).IsAssignableFrom(entityType.ClrType)
                                       select entityType)
            {
                modelBuilder.Entity(entityType.ClrType).Property<byte[]>(nameof(VersionedTable.RowVersion)).IsRowVersion();
            }
        }
    }
}
