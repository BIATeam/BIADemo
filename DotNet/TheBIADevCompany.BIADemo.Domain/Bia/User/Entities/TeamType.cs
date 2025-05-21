// <copyright file="TeamType.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.User.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The type of team.
    /// </summary>
    public class TeamType : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the roles with this type of team.
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
    }
}