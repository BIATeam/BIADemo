// <copyright file="SiteInfoDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Site
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The DTO used to manage site information.
    /// </summary>
    public class SiteInfoDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of site admin.
        /// </summary>
        public IEnumerable<MemberInfoDto> SiteAdmins { get; set; }
    }
}