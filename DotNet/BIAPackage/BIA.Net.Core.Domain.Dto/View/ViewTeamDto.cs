// <copyright file="ViewTeamDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.View
{
    /// <summary>
    /// ViewTeam Dto.
    /// </summary>
    public class ViewTeamDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public string TeamTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is the default one.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
