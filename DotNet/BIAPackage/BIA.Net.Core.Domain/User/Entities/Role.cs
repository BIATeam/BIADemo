// <copyright file="Role.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using System.Collections.Generic;

    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Translation.Entities;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class Role : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the label in english.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the collection of users.
        /// </summary>
        public ICollection<BaseEntityUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the members with this role.
        /// </summary>
        public virtual ICollection<MemberRole> MemberRoles { get; set; }

        /// <summary>
        /// Gets or sets the role translations.
        /// </summary>
        public virtual ICollection<RoleTranslation> RoleTranslations { get; set; }

        /// <summary>
        /// Gets or sets the team types for this role.
        /// </summary>
        public virtual ICollection<TeamType> TeamTypes { get; set; }

        /// <summary>
        /// Gets or sets the collection of notification roles.
        /// </summary>
        public ICollection<NotificationTeamRole> NotificationTeamRoles { get; set; }
    }
}