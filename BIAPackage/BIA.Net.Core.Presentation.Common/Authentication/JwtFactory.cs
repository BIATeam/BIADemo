// <copyright file="JwtFactory.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Common.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
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

        public ClaimsPrincipal GetPrincipalFromToken(string Token, string SecretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            try
            {
                var principal = tokenHandler.ValidateToken(Token, tokenValidationParameters, out var validatedToken);
                if (!ValidateSecurityAlgorithm(validatedToken))
                {
                    return null;
                };
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool ValidateSecurityAlgorithm(SecurityToken SecurityToken)
        {
            var res = (SecurityToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            return res;
        }

        /// <inheritdoc cref="IJwtFactory.GenerateClaimsIdentity"/>
        public ClaimsIdentity GenerateClaimsIdentity<TUserDataDto>(TokenDto<TUserDataDto> tokenDto) where TUserDataDto : UserDataDto
        {
            var claims = tokenDto.Permissions.Select(s => new Claim(ClaimTypes.Role, s)).ToList();
            claims.Add(new Claim(ClaimTypes.Sid, tokenDto.Id.ToString()));
            if (tokenDto.UserData != null)
            {
                claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(tokenDto.UserData)));
            }
            return new ClaimsIdentity(new GenericIdentity(tokenDto.Login, "Token"), claims);
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

        /// <inheritdoc cref="IJwtFactory.GenerateAuthInfoAsync"/>
        public async Task<AuthInfoDTO<TUserDataDto, TAdditionalInfoDto>> GenerateAuthInfoAsync<TUserDataDto, TAdditionalInfoDto>(TokenDto<TUserDataDto> tokenDto, TAdditionalInfoDto additionalInfos, bool lightToken) 
            where TUserDataDto : UserDataDto
            where TAdditionalInfoDto : AdditionalInfoDto
        {
            var claimsIdentity = await Task.FromResult(this.GenerateClaimsIdentity(tokenDto));
            if (claimsIdentity == null)
            {
                throw new Exception("Unauthorized because claimsIdentity is null");
            }

            var response = new AuthInfoDTO<TUserDataDto, TAdditionalInfoDto>
            {
                Token = await this.GenerateEncodedTokenAsync(claimsIdentity),
                UncryptedToken = lightToken ? null : tokenDto,
                AdditionalInfos = lightToken? null : additionalInfos,
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