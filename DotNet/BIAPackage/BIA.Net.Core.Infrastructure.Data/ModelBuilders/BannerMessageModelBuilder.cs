namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Banner.Entities;
    using Microsoft.EntityFrameworkCore;

    public sealed class BannerMessageModelBuilder
    {
        /// <summary>
        /// Create the banner messages models.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BannerMessage>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<BannerMessage>().Property(x => x.Start).IsRequired();
            modelBuilder.Entity<BannerMessage>().Property(x => x.End).IsRequired();
            modelBuilder.Entity<BannerMessage>().Property(x => x.Type).IsRequired();
            modelBuilder.Entity<BannerMessage>().Property(x => x.RawContent).IsRequired();
        }
    }
}
