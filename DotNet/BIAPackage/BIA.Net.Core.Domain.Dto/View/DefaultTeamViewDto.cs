// <copyright file="DefaultTeamViewDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.View
{
    /// <summary>
    /// DefaultTeamView Dto.
    /// </summary>
    /// <seealso cref="DefaultViewDto" />
    public class DefaultTeamViewDto : DefaultViewDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public int TeamId { get; set; }
    }
}