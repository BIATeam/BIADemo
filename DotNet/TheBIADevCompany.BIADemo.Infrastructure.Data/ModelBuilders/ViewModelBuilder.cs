// <copyright file="ViewModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.View.Entities;

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
            CreateViewTeamModel(modelBuilder);
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
            modelBuilder.Entity<View>().HasData(new View
            {
                Id = -1, // the System view use negative Id. Warning verify that this id is never use for view in your project.
                ViewType = 0,
                TableId = "notificationsGrid",
                Name = "default",
                Preference = "{\"first\":0,\"rows\":10,\"sortField\":\"createdDate\",\"sortOrder\":-1,\"columnOrder\":[\"titleTranslated\",\"descriptionTranslated\",\"type\",\"read\",\"createdDate\",\"createdBy\"],\"selection\":[],\"filters\":{}}",
            });
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
        /// Create the model for view teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateViewTeamModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewTeam>().HasKey(mr => new { TeamId = mr.TeamId, ViewId = mr.ViewId });
            modelBuilder.Entity<ViewTeam>().Property(m => m.IsDefault).IsRequired();
            modelBuilder.Entity<ViewTeam>().HasOne(mr => mr.Team).WithMany(m => m.ViewTeams)
                .HasForeignKey(mr => mr.TeamId);
            modelBuilder.Entity<ViewTeam>().HasOne(mr => mr.View).WithMany(m => m.ViewTeams)
                .HasForeignKey(mr => mr.ViewId);
        }
    }
}