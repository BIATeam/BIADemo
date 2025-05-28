// <copyright file="TeamFixableArchivable.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using System;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The team.
    /// </summary>
    public class TeamFixableArchivable : TeamFixable, IEntityArchivable
    {
        /// <summary>
        /// Gets or sets the is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the archived date.
        /// </summary>
        public DateTime? ArchivedDate { get; set; }
    }
}