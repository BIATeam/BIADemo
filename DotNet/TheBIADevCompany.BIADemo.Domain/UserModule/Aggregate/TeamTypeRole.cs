// <copyright file="TeamTypeRole.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity member role.
    /// </summary>
    public class TeamTypeRole : VersionedTable
    {
        /// <summary>
        /// Gets or sets the member.
        /// </summary>
        public virtual TeamType TeamType { get; set; }

        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }
    }
}