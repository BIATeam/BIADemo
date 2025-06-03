// <copyright file="ViewUser.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.View.Entities
{
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The mapping entity between users and views.
    /// </summary>
    public class ViewUser : VersionedTable
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual BaseUser User { get; set; }

        /// <summary>
        /// Gets or sets the view identifier.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        public virtual View View { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is the default one.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}