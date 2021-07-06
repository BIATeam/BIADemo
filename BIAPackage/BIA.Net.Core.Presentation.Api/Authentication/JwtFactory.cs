// <copyright file="JwtFactory.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    /// <summary>
    /// The factory used for JWT.
    /// </summary>
    public class JwtFactory : IJwtFactory
    {
        /// <summary>
        /// The current JWT options.
        /// </summary>
        private readonly JwtOptions jwtOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtFactory"/> class.
        /// </summary>
        /// <param name="jwtOptions">The JWT options.</param>
        public JwtFactory(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(this.jwtOptions);
        }

        /// <inheritdoc cref="IJwtFactory.GenerateClaimsIdentity"/>
        public ClaimsIdentity GenerateClaimsIdentity(string userName, int id, IEnumerable<string> roles, object userData = null)
        {
            var claims = roles.Select(s => new Claim(ClaimTypes.Role, s)).ToList();
            claims.Add(new Claim(ClaimTypes.Sid, id.ToString()));
            if (userData != null)
            {
                claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userData)));
            }
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), claims);
        }

        /// <inheritdoc cref="IJwtFactory.GenerateEncodedTokenAsync"/>
        public async Task<string> GenerateEncodedTokenAsync(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            claims.AddRange(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, await this.jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(this.jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            });

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: this.jwtOptions.Issuer,
                audience: this.jwtOptions.Audience,
                claims: claims,
                notBefore: this.jwtOptions.NotBefore,
                expires: this.jwtOptions.Expiration,
                signingCredentials: this.jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        /// <inheritdoc cref="IJwtFactory.GenerateJwtAsync"/>
        public async Task<object> GenerateJwtAsync(ClaimsIdentity identity, object additionalInfos)
        {
            var response = new
            {
                token = await this.GenerateEncodedTokenAsync(identity),
                permissions = identity.Claims.Where(w => w.Type == ClaimTypes.Role).Select(s => s.Value).ToList(),
                additionalInfos,
            };

            return response;
        }

        /// <summary>
        /// Throw an exception if a JWT option is not correctly set.
        /// </summary>
        /// <param name="options">The JWT options to check.</param>
        private static void ThrowIfInvalidOptions(JwtOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException($"The value of {nameof(options.ValidFor)} must be a non-zero TimeSpan.", nameof(options));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(options), $"The attribute {nameof(options.SigningCredentials)} is null.");
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(options), $"The attribute {nameof(options.JtiGenerator)} is null.");
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}