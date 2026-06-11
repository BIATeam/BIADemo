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
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(e => typeof(IEntityVersioned).IsAssignableFrom(e.ClrType)))
            {
                var clrType = entityType.ClrType;
                EntityTypeBuilder entityTypeBuilder = modelBuilder.Entity(clrType);
                var rowVersionPropertyName = clrType.GetBiaRowVersionProperty(DbProvider.SqlServer)?.Name ?? nameof(IEntityVersioned.RowVersion);
                var rowVersionXminPropertyName = clrType.GetBiaRowVersionProperty(DbProvider.PostGreSql)?.Name ?? nameof(IEntityVersioned.RowVersionXmin);

                if (databaseFacade.IsNpgsql())
                {
                    entityTypeBuilder.Property<uint>(rowVersionXminPropertyName).IsRowVersion();
                    if (!IsOwnedByBaseEntityType(entityType, rowVersionPropertyName))
                    {
                        entityTypeBuilder.Ignore(rowVersionPropertyName);
                    }
                }
                else
                {
                    entityTypeBuilder.Property<byte[]>(rowVersionPropertyName).HasColumnName(nameof(IEntityVersioned.RowVersion)).IsRowVersion();
                    if (!IsOwnedByBaseEntityType(entityType, rowVersionPropertyName))
                    {
                        entityTypeBuilder.Ignore(rowVersionXminPropertyName);
                    }
                }
            }
        }

        private static bool IsOwnedByBaseEntityType(IMutableEntityType entityType, string propertyName)
        {
            var baseType = entityType.BaseType;

            while (baseType != null)
            {
                if (baseType.FindProperty(propertyName) != null)
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}
