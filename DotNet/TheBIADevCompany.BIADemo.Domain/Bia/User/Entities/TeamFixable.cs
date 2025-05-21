// <copyright file="TeamFixable.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.User.Entities
{
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The team.
    /// </summary>
    public class TeamFixable : Team, IEntityFixable
    {
        /// <summary>
        /// Gets or sets the is fixed.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Gets or sets the fixed date.
        /// </summary>
        public DateTime? FixedDate { get; set; }
    }
}