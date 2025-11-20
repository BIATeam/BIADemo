namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Annoucement.Entities;
    using BIA.Net.Core.Domain.Translation.Entities;
    using Microsoft.EntityFrameworkCore;

    public static class AnnoucementModelBuilder
    {
        /// <summary>
        /// Create the annoucements models.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Annoucement>().Property(x => x.Start).IsRequired();
            modelBuilder.Entity<Annoucement>().Property(x => x.End).IsRequired();
            modelBuilder.Entity<Annoucement>().Property(x => x.RawContent).IsRequired();
            modelBuilder.Entity<Annoucement>()
                .HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeId)
                .OnDelete(DeleteBehavior.ClientCascade)
                .IsRequired();

            modelBuilder.Entity<AnnoucementType>().HasData(new AnnoucementType { Id = Common.Enum.BiaAnnoucementType.Info });
            modelBuilder.Entity<AnnoucementType>().HasData(new AnnoucementType { Id = Common.Enum.BiaAnnoucementType.Warning });
        }
    }
}
