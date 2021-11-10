// <copyright file="MemberFilterDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The member filter DTO.
    /// </summary>
    public class MemberFilterDto : LazyLoadDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int SiteId { get; set; }
    }

    /// <summary>
    /// The member filter DTO used for CSV Export.
    /// </summary>
    public class MemberFileFilterDto : MemberFilterDto
    {
        public Dictionary<string, string> Columns { get; set; }
    }
}