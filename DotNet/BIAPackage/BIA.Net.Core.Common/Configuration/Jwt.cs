// <copyright file="Authentication.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The authentication configuration.
    /// </summary>
    public class Jwt
    {
        /// <summary>
        /// The jwt issuer Name (name of the Api application).
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The Secret Key to crypte the token.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the "aud" (Audience) Claim - The "aud" (audience) claim identifies the
        /// recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the "exp" (Expiration Time) Claim - The "exp" (expiration time) claim
        /// identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime Expiration => this.IssuedAt.Add(this.ValidFor);

        /// <summary>
        /// Gets or sets the "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the
        /// time at which the JWT was issued.
        /// </summary>
        public DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID).
        /// </summary>
        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());

        /// <summary>
        /// Gets or sets the "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the
        /// time before which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the "sub" (Subject) Claim - The "sub" (subject) claim identifies the
        /// principal that is the subject of the JWT.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Set the timespan the token will be valid for (default is 120 min).
        /// </summary>
        public TimeSpan ValidFor => TimeSpan.FromMinutes(this.ValidTime == 0 ? 120 : this.ValidTime);

        /// <summary>
        /// The time the token will be valid.
        /// </summary>
        public int ValidTime { get; set; }
    }
}