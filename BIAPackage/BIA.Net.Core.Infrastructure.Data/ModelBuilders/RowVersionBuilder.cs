using BIA.Net.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    public static class RowVersionBuilder
    {
        public static void CreateRowVersion(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(VersionedTable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Property<byte[]>(nameof(VersionedTable.RowVersion)).IsRowVersion();
                }
            }
        }
    }
}
