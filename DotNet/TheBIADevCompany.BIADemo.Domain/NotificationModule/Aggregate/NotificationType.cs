// <copyright file="NotificationType.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The NotificationType entity.
    /// </summary>
    public class NotificationType : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the notification code;s
        /// e.g: Task, Info, Success, Warning, Error.
        /// </summary>
        public string Code { get; set; }
    }
}