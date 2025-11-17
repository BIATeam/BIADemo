namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Translation.Entities;
    using Microsoft.EntityFrameworkCore;

    public static class BannerMessageModelBuilder
    {
        /// <summary>
        /// Create the banner messages models.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BannerMessage>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<BannerMessage>().Property(x => x.Start).IsRequired();
            modelBuilder.Entity<BannerMessage>().Property(x => x.End).IsRequired();
            modelBuilder.Entity<BannerMessage>().Property(x => x.RawContent).IsRequired();
            modelBuilder.Entity<BannerMessage>()
                .HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .IsRequired();

            modelBuilder.Entity<BannerMessageType>().HasData(new BannerMessageType { Id = Common.Enum.BiaBannerMessageType.Info });
            modelBuilder.Entity<BannerMessageType>().HasData(new BannerMessageType { Id = Common.Enum.BiaBannerMessageType.Warning });
        }
    }
}
