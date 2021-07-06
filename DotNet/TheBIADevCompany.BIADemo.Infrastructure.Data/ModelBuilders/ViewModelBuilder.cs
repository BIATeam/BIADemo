// <copyright file="ViewModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for view domain.
    /// </summary>
    public static class ViewModelBuilder
    {
        /// <summary>
        /// Create the view model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateViewModel(modelBuilder);
            CreateViewUserModel(modelBuilder);
            CreateViewSiteModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for views.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateViewModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<View>().HasKey(m => m.Id);
            modelBuilder.Entity<View>().Property(m => m.TableId).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<View>().Property(m => m.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<View>().Property(m => m.Description).HasMaxLength(500);
            modelBuilder.Entity<View>().Property(m => m.Preference).IsRequired();
            modelBuilder.Entity<View>().Property(m => m.ViewType).IsRequired();
        }

        /// <summary>
        /// Create the model for view users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateViewUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewUser>().HasKey(mr => new { UserId = mr.UserId, ViewId = mr.ViewId });
            modelBuilder.Entity<ViewUser>().Property(m => m.IsDefault).IsRequired();
            modelBuilder.Entity<ViewUser>().HasOne(mr => mr.User).WithMany(m => m.ViewUsers)
                .HasForeignKey(mr => mr.UserId);
            modelBuilder.Entity<ViewUser>().HasOne(mr => mr.View).WithMany(m => m.ViewUsers)
                .HasForeignKey(mr => mr.ViewId);
        }

        /// <summary>
        /// Create the model for view sites.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateViewSiteModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewSite>().HasKey(mr => new { SiteId = mr.SiteId, ViewId = mr.ViewId });
            modelBuilder.Entity<ViewSite>().Property(m => m.IsDefault).IsRequired();
            modelBuilder.Entity<ViewSite>().HasOne(mr => mr.Site).WithMany(m => m.ViewSites)
                .HasForeignKey(mr => mr.SiteId);
            modelBuilder.Entity<ViewSite>().HasOne(mr => mr.View).WithMany(m => m.ViewSites)
                .HasForeignKey(mr => mr.ViewId);
        }
    }
}