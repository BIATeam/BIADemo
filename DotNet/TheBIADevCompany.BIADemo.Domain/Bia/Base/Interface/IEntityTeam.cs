// <copyright file="IEntityTeam.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.Base.Interface
{
    using System.Collections.Generic;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;

    /// <summary>
    /// The team.
    /// </summary>
    public interface IEntityTeam
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the team type.
        /// </summary>
        public TeamType TeamType { get; set; }

        /// <summary>
        /// Gets or sets the team type id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        public ICollection<Member> Members { get; set; }
    }
}