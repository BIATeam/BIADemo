// <copyright file="BaseDtoVersionedTeam.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The DTO used to manage site.
    /// </summary>
    public class BaseDtoVersionedTeam : BaseDtoVersioned<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the site is the default one.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public ICollection<RoleDto> Roles { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public int ParentTeamId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string ParentTeamTitle { get; set; }

        /// <summary>
        /// Gets or sets if team is updatable by the user.
        /// </summary>
        public bool CanUpdate { get; set; }

        /// <summary>
        /// Gets or sets if user can access list member.
        /// </summary>
        public bool CanMemberListAccess { get; set; }

        /// <summary>
        /// Gets or sets the list of admin.
        /// </summary>
        public IEnumerable<OptionDto> Admins { get; set; }
    }
}