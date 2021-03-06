// <copyright file="DefaultTeamViewDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.View
{
    /// <summary>
    /// DefaultTeamView Dto.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Domain.Dto.View.DefaultViewDto" />
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