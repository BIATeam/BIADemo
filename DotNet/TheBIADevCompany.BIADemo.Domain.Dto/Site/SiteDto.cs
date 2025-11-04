// <copyright file="SiteDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Site
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to manage site.
    /// </summary>
#pragma warning disable S2094 // Classes should not be empty
    public class SiteDto : BaseDtoVersionedTeam
#pragma warning restore S2094 // Classes should not be empty
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string UniqueIdentifier { get; set; }
    }
}