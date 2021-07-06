// <copyright file="JwtIssuerOptions.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Authentication
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// Class representing the JWT options.
    /// </summary>
    public class JwtOptions
    {
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
        /// Gets or sets the "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the
        /// principal that issued the JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public Func<Task<string>> JtiGenerator =>
          () => Task.FromResult(Guid.NewGuid().ToString());

        /// <summary>
        /// Gets or sets the "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the
        /// time before which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

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