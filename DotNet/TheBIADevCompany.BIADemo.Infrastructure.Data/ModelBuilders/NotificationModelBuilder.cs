// <copyright file="NotificationModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;

    /// <summary
    /// Class used to update the model builder for notification domain.
    /// </summary>
    public class NotificationModelBuilder : BaseNotificationModelBuilder
    {
        /// <inheritdoc/>
        protected override void CreateNotificationModel(ModelBuilder modelBuilder)
        {
            base.CreateNotificationModel(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationModelData(ModelBuilder modelBuilder)
        {
            base.CreateNotificationModelData(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationTypeModel(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTypeModel(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationTypeModelData(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTypeModelData(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationUserModel(ModelBuilder modelBuilder)
        {
            base.CreateNotificationUserModel(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationUserModelData(ModelBuilder modelBuilder)
        {
            base.CreateNotificationUserModelData(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationTeamModel(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTeamModel(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationTeamModelData(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTeamModelData(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationTeamRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTeamRoleModel(modelBuilder);
        }

        /// <inheritdoc/>
        protected override void CreateNotificationTeamRoleModelData(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTeamRoleModelData(modelBuilder);
        }
    }
}