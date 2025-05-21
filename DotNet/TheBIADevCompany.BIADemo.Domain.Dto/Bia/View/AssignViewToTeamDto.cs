// <copyright file="AssignViewToTeamDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Bia.View
{
    /// <summary>
    /// AssignViewToSite Dto.
    /// </summary>
    public class AssignViewToTeamDto
    {
        /// <summary>
        /// Gets or sets the view identifier.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is assign.
        /// </summary>
        public bool IsAssign { get; set; }
    }
}
