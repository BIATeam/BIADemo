// <copyright file="RoleApi.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The Role Api configuration.
    /// </summary>
    public class RoleApi : BiaWebApi.BiaWebApi
    {
        /// <summary>
        /// Define if user roles for the application should be retrieved from an API.
        /// </summary>
        public bool GetRolesFromApi { get; set; }

        /// <summary>
        /// Gets or sets the url of the API endpoint to retrieve the roles.
        /// </summary>
        public string EndpointUrl { get; set; }
    }
}