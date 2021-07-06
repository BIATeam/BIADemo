// <copyright file="DefaultSiteViewDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.View
{
    /// <summary>
    /// DefaultSiteView Dto.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Domain.Dto.View.DefaultViewDto" />
    public class DefaultSiteViewDto : DefaultViewDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public int SiteId { get; set; }
    }
}