// <copyright file="SearchUserResponseDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak.SearchUserResponse
{
    using Newtonsoft.Json;

    /// <summary>
    /// SearchUserResponse Dto.
    /// </summary>
    internal class SearchUserResponseDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        internal string Id { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [JsonProperty("username")]
        internal string Username { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SearchUserResponseDto"/> is enabled.
        /// </summary>
        [JsonProperty("enabled")]
        internal bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [JsonProperty("firstName")]
        internal string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [JsonProperty("lastName")]
        internal string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [JsonProperty("email")]
        internal string Email { get; set; }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        [JsonProperty("attributes")]
        internal AttributesDto Attribute { get; set; }
    }
}
