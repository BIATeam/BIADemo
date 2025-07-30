// <copyright file="IBaseNotificationQueryCustomizer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;

    /// <summary>
    /// Interface BaseNotification Query Customizer.
    /// </summary>
    /// <typeparam name="TBaseNotification">The type of the notification entity, which must inherit from <see cref="BaseNotification"/>.</typeparam>
    public interface IBaseNotificationQueryCustomizer<TBaseNotification> : IQueryCustomizer<TBaseNotification>
        where TBaseNotification : BaseNotification
    {
    }
}
