// <copyright file="SiteModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for site domain.
    /// </summary>
    public static class SiteModelBuilder
    {
        /// <summary>
        /// Create the model for sites.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateSiteModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>().HasKey(s => s.Id);
            modelBuilder.Entity<Site>().Property(s => s.Title).IsRequired().HasMaxLength(256);
        }
    }
}