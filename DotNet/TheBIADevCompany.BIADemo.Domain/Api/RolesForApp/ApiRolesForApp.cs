// BIADemo only
// <copyright file="ApiRolesForApp.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Api.RolesForApp
{
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a the list of sites, programs and respective user roles.
    /// </summary>
    public class ApiRolesForApp
    {
        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        [BiaDtoField(Required = true, ItemType = "Site")]
        public ICollection<ApiSite> Sites { get; set; }
    }
}
