// <copyright file="RowVersionBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System;
    using System.Linq;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Helpers;
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
                                    where typeof(IEntityVersioned).IsAssignableFrom(entityType.ClrType)
                                    select entityType.ClrType)
            {
                EntityTypeBuilder entityTypeBuilder = modelBuilder.Entity(clrType);
                var rowVersionPropertyName = clrType.GetBiaRowVersionProperty(DbProvider.SqlServer)?.Name ?? nameof(IEntityVersioned.RowVersion);
                var rowVersionXminPropertyName = clrType.GetBiaRowVersionProperty(DbProvider.PostGreSql)?.Name ?? nameof(IEntityVersioned.RowVersionXmin);

                if (databaseFacade.IsNpgsql())
                {
                    entityTypeBuilder.Property<uint>(rowVersionXminPropertyName).IsRowVersion();
                    entityTypeBuilder.Ignore(rowVersionPropertyName);
                }
                else
                {
                    entityTypeBuilder.Property<byte[]>(rowVersionPropertyName).HasColumnName(nameof(IEntityVersioned.RowVersion)).IsRowVersion();
                    entityTypeBuilder.Ignore(rowVersionXminPropertyName);
                }
            }
        }
    }
}
