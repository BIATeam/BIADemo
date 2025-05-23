// <copyright file="NotificationQueryCustomizer.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.QueryCustomizer.Bia
{
    using System.Linq;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Bia.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    /// <summary>
    /// Class use to customize the EF request on Member entity.
    /// </summary>
    public class NotificationQueryCustomizer : TQueryCustomizer<Notification>, INotificationQueryCustomizer
    {
        /// <inheritdoc/>
        public override IQueryable<Notification> CustomizeAfter(IQueryable<Notification> objectSet, string queryMode)
        {
            if (queryMode == QueryMode.Update)
            {
                return objectSet
                    .Include(n => n.NotifiedTeams).ThenInclude(nt => nt.Roles)
                    .Include(n => n.NotifiedUsers)
                    .Include(n => n.NotificationTranslations);
            }

            return objectSet;
        }
    }
}
