// <copyright file="View.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.View.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.View.Models;

    /// <summary>
    /// The View entity.
    /// </summary>
    public class View : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the view description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the table preference.
        /// </summary>
        public string Preference { get; set; }

        /// <summary>
        /// Gets or sets view type.
        /// </summary>
        public ViewType ViewType { get; set; }

        /// <summary>
        /// Gets or sets the collection of view user.
        /// </summary>
        public ICollection<ViewUser> ViewUsers { get; set; }

        /// <summary>
        /// Gets or sets the collection of view site.
        /// </summary>
        public ICollection<ViewTeam> ViewTeams { get; set; }
    }
}