// <copyright file="Notification.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Notification.Entities
{
    // Begin BIADemo
    using System;

    // End BIADemo
    using BIA.Net.Core.Domain.Notification.Entities;

    /// <summary>
    /// Notification.
    /// </summary>
    public class Notification : BaseNotification
    {
        // Begin BIADemo

        /// <summary>
        /// Gets or sets the date the notification was acknowledged by the user.
        /// </summary>
        public DateTimeOffset? AcknowledgedAt { get; set; }

        // End BIADemo
    }
}
