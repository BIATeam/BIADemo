// <copyright file="INotificationQueryCustomizer.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;

    /// <summary>
    /// interface use to customize the request on Member entity.
    /// </summary>
    public interface INotificationQueryCustomizer : IQueryCustomizer<Notification>
    {
    }
}
