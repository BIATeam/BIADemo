// <copyright file="AnnouncementModelBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.Announcement.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The announcement model builder.
    /// </summary>
    public static class AnnouncementModelBuilder
    {
        /// <summary>
        /// Create the announcements models.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>().Property(x => x.Start).IsRequired();
            modelBuilder.Entity<Announcement>().Property(x => x.End).IsRequired();
            modelBuilder.Entity<Announcement>().Property(x => x.RawContent).IsRequired();
            modelBuilder.Entity<Announcement>()
                .HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .IsRequired();

            modelBuilder.Entity<AnnouncementType>().HasData(new AnnouncementType { Id = Common.Enum.BiaAnnouncementType.Information });
            modelBuilder.Entity<AnnouncementType>().HasData(new AnnouncementType { Id = Common.Enum.BiaAnnouncementType.Warning });
        }
    }
}
