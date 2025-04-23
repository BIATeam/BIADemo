// <copyright file="TokenRequestDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Dto.Keycloak
{
    using Newtonsoft.Json;

    /// <summary>
    /// Token Response Dto.
    /// </summary>
    public class TokenRequestDto
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the type of the grant.
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
