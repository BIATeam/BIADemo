// <copyright file="User.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.View.Entities;
    using global::Audit.EntityFramework;

    /// <summary>
    /// The user entity.
    /// </summary>
    [AuditInclude]
    public class User : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the DAI date.
        /// </summary>
        [AuditIgnore]
        public DateTime DaiDate { get; set; }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        public virtual ICollection<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where the user is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the last login date.
        /// </summary>
        [AuditIgnore]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of view user.
        /// </summary>
        public ICollection<ViewUser> ViewUsers { get; set; }

        /// <summary>
        /// Gets or sets the collection of notifications.
        /// </summary>
        public ICollection<NotificationUser> NotificationUsers { get; set; }

        /// <summary>
        /// Gets or sets the collection of roles.
        /// </summary>
        public ICollection<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the collection of default teams.
        /// </summary>
        public ICollection<UserDefaultTeam> DefaultTeams { get; set; }
    }
}