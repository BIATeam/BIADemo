// <copyright file="FileDownloadDataModelBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.File.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class used to update the model builder for file download data domain.
    /// </summary>
    public static class FileDownloadDataModelBuilder
    {
        /// <summary>
        /// Create the file download data model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateFileDownloadDataModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for file download data.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateFileDownloadDataModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileDownloadData>().HasKey(m => m.Id);
            modelBuilder.Entity<FileDownloadData>().Property(m => m.FileName).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<FileDownloadData>().Property(m => m.FileContentType).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<FileDownloadData>().Property(m => m.FilePath).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<FileDownloadData>().Property(m => m.RequestDateTime).IsRequired();
            modelBuilder.Entity<FileDownloadData>().Property(m => m.RequestByUserId).IsRequired();
            modelBuilder.Entity<FileDownloadData>()
                .HasOne(m => m.RequestByUser)
                .WithMany()
                .HasForeignKey(m => m.RequestByUserId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
