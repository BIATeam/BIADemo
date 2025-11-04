// <copyright file="ApiProgram.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Api.RolesForApp
{
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a program.
    /// </summary>
    public class ApiProgram
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of appRoles.
        /// </summary>
        [BiaDtoField(Required = false, ItemType = "AppRole")]
        public ICollection<string> AppRoles { get; set; }
    }
}
