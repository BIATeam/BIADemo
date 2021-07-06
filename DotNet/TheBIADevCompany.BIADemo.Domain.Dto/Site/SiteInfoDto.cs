// <copyright file="SiteInfoDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Site
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used to manage site information.
    /// </summary>
    public class SiteInfoDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of site admin.
        /// </summary>
        public IEnumerable<SiteMemberDto> SiteAdmins { get; set; }
    }
}