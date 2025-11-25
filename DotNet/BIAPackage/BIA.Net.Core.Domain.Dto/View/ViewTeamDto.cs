// <copyright file="ViewTeamDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.View
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// ViewTeam Dto.
    /// </summary>
    public class ViewTeamDto : BaseDto<int>
    {
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
