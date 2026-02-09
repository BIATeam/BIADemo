// <copyright file="JwtFactory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;

    /// <summary>
    /// The factory used for JWT.
    /// </summary>
    public class JwtFactory : IJwtFactory
    {
        /// <summary>
        /// The current JWT options.
        /// </summary>
        private readonly Jwt jwt;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtFactory"/> class.
        /// </summary>
        /// <param name="jwtOptions">The JWT options.</param>
        public JwtFactory(IOptions<Jwt> jwtOptions)
        {
            this.jwt = jwtOptions.Value;

            SymmetricSecurityKey signingKey = new(Encoding.ASCII.GetBytes(this.jwt.SecretKey));
            this.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            ThrowIfInvalidOptions(this.jwt);
        }

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Check if a permission is in a given jwt token.
        /// </summary>
        /// <param name="jwtToken">The jwt token.</param>
        /// <param name="permissionToCheck">The permission to look for in the token.</param>
        /// <returns>If the role is in the token.</returns>
        public static bool HasPermission(string jwtToken, int permissionToCheck)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);
            return token.Claims.Any(c => c.Type == BiaClaimsPrincipal.PermissionIds && c.Value == permissionToCheck.ToString());
        }

        /// <summary>
        /// Extract the claims from token.
        /// </summary>
        /// <param name="token">the token.</param>
        /// <param name="secretKey">the secret key.</param>
        /// <returns>the claims.</returns>
        public ClaimsPrincipal GetPrincipalFromToken(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = this.SigningCredentials.Key,
                ValidateLifetime = false, // here we are saying that we don't care about the token's expiration date
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!ValidateSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public ClaimsIdentity GenerateClaimsIdentity<TUserDataDto>(TokenDto<TUserDataDto> tokenDto)
            where TUserDataDto : BaseUserDataDto
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, tokenDto.Id.ToString()),
                new(BiaClaimsPrincipal.PermissionIds, JsonConvert.SerializeObject(tokenDto.Permissions)),
            };

            claims.AddRange(tokenDto.RoleIds.Select(s => new Claim(BiaClaimsPrincipal.RoleId, s.ToString())));

            if (tokenDto.UserData != null)
            {
                claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(tokenDto.UserData)));
            }

            return new ClaimsIdentity(new GenericIdentity(tokenDto.IdentityKey, "Token"), claims);
        }

        /// <inheritdoc />
        public async Task<string> GenerateEncodedTokenAsync<TUserDataDto>(ClaimsIdentity identity)
            where TUserDataDto : BaseUserDataDto
        {
            return await this.GenerateEncodedTokenAsync<TUserDataDto>(identity, null);
        }

        /// <inheritdoc />
        public async Task<AuthInfoDto<TAdditionalInfoDto>> GenerateAuthInfoAsync<TUserDataDto, TAdditionalInfoDto>(TokenDto<TUserDataDto> tokenDto, TAdditionalInfoDto additionalInfos, LoginParamDto loginParam)
            where TUserDataDto : BaseUserDataDto
            where TAdditionalInfoDto : BaseAdditionalInfoDto
        {
            var claimsIdentity = await Task.FromResult(this.GenerateClaimsIdentity(tokenDto));
            if (claimsIdentity == null)
            {
                throw new UnauthorizedException("Unauthorized because claimsIdentity is null");
            }

            var response = new AuthInfoDto<TAdditionalInfoDto>
            {
                Token = await this.GenerateEncodedTokenAsync(claimsIdentity, tokenDto),
                AdditionalInfos = !loginParam.AdditionalInfos ? null : additionalInfos,
            };

            return response;
        }

        private static bool ValidateSecurityAlgorithm(SecurityToken securityToken)
        {
            var res = (securityToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            return res;
        }

        /// <summary>
        /// Throw an exception if a JWT option is not correctly set.
        /// </summary>
        /// <param name="options">The JWT options to check.</param>
        private static void ThrowIfInvalidOptions(Jwt options)
        {
            ArgumentNullException.ThrowIfNull(options);

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException($"The value of {nameof(options.ValidFor)} must be a non-zero TimeSpan.", nameof(options));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(options), $"The attribute {nameof(options.JtiGenerator)} is null.");
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               DateTimeOffset.UnixEpoch)
                              .TotalSeconds);

        private async Task<string> GenerateEncodedTokenAsync<TUserDataDto>(ClaimsIdentity identity, TokenDto<TUserDataDto> tokenDto)
            where TUserDataDto : BaseUserDataDto
        {
            var claims = identity.Claims.ToList();
            claims.AddRange(
            [
                new(JwtRegisteredClaimNames.Sub, identity.Name),
                new(JwtRegisteredClaimNames.Jti, await this.jwt.JtiGenerator()),
                new(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(this.jwt.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            ]);

            if (tokenDto != null)
            {
                claims.AddRange(
                [
                    new(ClaimTypes.GivenName, tokenDto.UserData.FirstName),
                    new(ClaimTypes.Surname, tokenDto.UserData.LastName),
                ]);
            }

            // Create the JWT security token and encode it.
            var jwtSecurity = new JwtSecurityToken(
                issuer: this.jwt.Issuer,
                audience: this.jwt.Audience,
                claims: claims,
                notBefore: this.jwt.NotBefore,
                expires: this.jwt.Expiration,
                signingCredentials: this.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurity);

            return encodedJwt;
        }
    }
}