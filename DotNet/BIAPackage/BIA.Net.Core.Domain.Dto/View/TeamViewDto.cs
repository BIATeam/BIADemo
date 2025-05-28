// <copyright file="TeamViewDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.View
{
    /// <summary>
    /// The DTO used to represent a siteView.
    /// </summary>
    public class TeamViewDto : ViewDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int TeamId { get; set; }
    }
}