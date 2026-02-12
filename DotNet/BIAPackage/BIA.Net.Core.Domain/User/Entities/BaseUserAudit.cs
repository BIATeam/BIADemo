// <copyright file="BaseUserAudit.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// The user entity.
    /// </summary>
    /// <typeparam name="TUser">User audit entity type.</typeparam>
    public abstract class BaseUserAudit<TUser> : AuditKeyedEntity<TUser, int, int>
        where TUser : BaseEntityUser
    {
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
        /// Gets or sets the domain.
        /// </summary>
        public string Domain { get; set; }
    }
}