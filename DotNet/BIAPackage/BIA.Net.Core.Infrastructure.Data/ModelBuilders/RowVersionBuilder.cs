// <copyright file="RowVersionBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System.Linq;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Entity.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    /// <summary>
    /// Build the column RowVersion in each table.
    /// </summary>
    public static class RowVersionBuilder
    {
        /// <summary>
        /// Build the column RowVersion in each table.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateRowVersion(ModelBuilder modelBuilder, DatabaseFacade databaseFacade)
        {
            foreach (var entityType in from entityType in modelBuilder.Model.GetEntityTypes()
                                       where typeof(VersionedTable).IsAssignableFrom(entityType.ClrType) ||
                                       typeof(IEntityVersioned).IsAssignableFrom(entityType.ClrType)
                                       select entityType)
            {
                if (databaseFacade.IsNpgsql())
                {
                    modelBuilder.Entity(entityType.ClrType).Property<uint>(nameof(VersionedTable.RowVersionXmin)).IsRowVersion();
                    modelBuilder.Entity(entityType.ClrType).Ignore(nameof(VersionedTable.RowVersion));
                }
                else
                {
                    modelBuilder.Entity(entityType.ClrType).Property<byte[]>(nameof(VersionedTable.RowVersion)).IsRowVersion();
                    modelBuilder.Entity(entityType.ClrType).Ignore(nameof(VersionedTable.RowVersionXmin));
                }
            }
        }
    }
}
