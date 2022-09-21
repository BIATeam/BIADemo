// <copyright file="SiteFilterDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Site
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The site filter DTO.
    /// </summary>
    public class SiteFilterDto : LazyLoadDto
    {
        /// <summary>
        /// Gets or sets the advancedFilter.
        /// </summary>
        public SiteAdvancedFilterDto AdvancedFilter { get; set; }
    }
}