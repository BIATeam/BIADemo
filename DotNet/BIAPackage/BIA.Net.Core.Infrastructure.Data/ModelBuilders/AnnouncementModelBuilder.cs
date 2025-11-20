namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Translation.Entities;
    using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<AnnouncementType>().HasData(new AnnouncementType { Id = Common.Enum.BiaAnnouncementType.Info });
            modelBuilder.Entity<AnnouncementType>().HasData(new AnnouncementType { Id = Common.Enum.BiaAnnouncementType.Warning });
        }
    }
}
