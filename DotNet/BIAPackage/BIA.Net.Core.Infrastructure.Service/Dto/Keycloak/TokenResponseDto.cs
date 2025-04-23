// <copyright file="TokenResponseDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Dto.Keycloak
{
    using Newtonsoft.Json;

    /// <summary>
    /// Token Request Dto.
    /// </summary>
    public class TokenResponseDto
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the expires in.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the refresh expires in.
        /// </summary>
        [JsonProperty("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the not before policy.
        /// </summary>
        [JsonProperty("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        /// <summary>
        /// Gets or sets the state of the session.
        /// </summary>
        [JsonProperty("session_state")]
        public string SessionState { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
