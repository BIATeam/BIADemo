// <copyright file="NotificationQueryCustomizer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories.QueryCustomizer
{
    using System.Linq;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using Microsoft.EntityFrameworkCore;

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
