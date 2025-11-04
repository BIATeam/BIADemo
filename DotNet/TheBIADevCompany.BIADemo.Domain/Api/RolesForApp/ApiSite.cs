// <copyright file="ApiSite.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Api.RolesForApp
{
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to send site.
    /// </summary>
    public class ApiSite
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the list of programs.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "Program")]
        public ICollection<ApiProgram> Programs { get; set; }

        /// <summary>
        /// Gets or sets the list of appRoles.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "AppRole")]
        public ICollection<string> AppRoles { get; set; }
    }
}