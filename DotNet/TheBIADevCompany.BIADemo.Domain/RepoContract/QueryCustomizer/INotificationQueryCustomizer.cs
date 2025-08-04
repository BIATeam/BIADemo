// <copyright file="INotificationQueryCustomizer.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract.QueryCustomizer
{
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    /// <summary>
    /// Interface Notification Query Customizer.
    /// </summary>
    public interface INotificationQueryCustomizer : IBaseNotificationQueryCustomizer<Notification>
    {
    }
}
