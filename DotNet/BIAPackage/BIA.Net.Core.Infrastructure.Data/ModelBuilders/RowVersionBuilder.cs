// <copyright file="RowVersionBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System;
    using System.Linq;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Entity.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Build the column RowVersion in each table.
    /// </summary>
    public static class RowVersionBuilder
    {
        /// <summary>
        /// Build the column RowVersion in each table.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="databaseFacade">The database facade.</param>
        public static void CreateRowVersion(ModelBuilder modelBuilder, DatabaseFacade databaseFacade)
        {
            foreach (Type clrType in from entityType in modelBuilder.Model.GetEntityTypes()
                                    where typeof(VersionedTable).IsAssignableFrom(entityType.ClrType)
                                    || typeof(IEntityVersioned).IsAssignableFrom(entityType.ClrType)
                                    select entityType.ClrType)
            {
                EntityTypeBuilder entityTypeBuilder = modelBuilder.Entity(clrType);

                if (databaseFacade.IsNpgsql())
                {
                    entityTypeBuilder.Property<uint>(nameof(VersionedTable.RowVersionXmin)).IsRowVersion();
                    entityTypeBuilder.Ignore(nameof(VersionedTable.RowVersion));
                }
                else
                {
                    entityTypeBuilder.Property<byte[]>(nameof(VersionedTable.RowVersion)).IsRowVersion();
                    entityTypeBuilder.Ignore(nameof(VersionedTable.RowVersionXmin));
                }
            }
        }
    }
}
