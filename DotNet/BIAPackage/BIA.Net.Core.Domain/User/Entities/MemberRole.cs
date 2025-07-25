// <copyright file="MemberRole.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity member role.
    /// </summary>
    public class MemberRole : VersionedTable
    {
        /// <summary>
        /// Gets or sets the member.
        /// </summary>
        public virtual Member Member { get; set; }

        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is the default one.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}