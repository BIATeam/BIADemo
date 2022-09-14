// <copyright file="IJwtFactory.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Authentication
{
    using BIA.Net.Core.Domain.Dto.User;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the JWT factory.
    /// </summary>
    public interface IJwtFactory
    {
        /// <summary>
        /// Decrypt the token
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="SecretKey"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromToken(string Token, string SecretKey);

        /// <summary>
        /// Generate the identity for a user.
        /// </summary>
        /// <param name="tokenDto">The token data.</param>
        ClaimsIdentity GenerateClaimsIdentity<TUserDataDto>(TokenDto<TUserDataDto> tokenDto) where TUserDataDto : UserDataDto;

        /// <summary>
        /// Generate an encoded JWT.
        /// </summary>
        /// <param name="identity">The identity of the current user.</param>
        /// <returns>The encoded JWT as string.</returns>
        Task<string> GenerateEncodedTokenAsync(ClaimsIdentity identity);

        /// <summary>
        /// Generate a JWT.
        /// </summary>
        /// <param name="tokenDto">The token not uncrypted.</param>
        /// <param name="additionalInfos">Additionnal Info for front</param>
        /// <param name="fullToken">Additionnal Info for front</param>
        /// The additional information we want to let visible in the token.
        /// </param>
        /// <returns>The JWT as string.</returns>
        Task<AuthInfoDTO<TUserDataDto, TAdditionalInfoDto>> GenerateAuthInfoAsync<TUserDataDto, TAdditionalInfoDto>(TokenDto<TUserDataDto> tokenDto, TAdditionalInfoDto additionalInfos, bool fullToken)
            where TUserDataDto : UserDataDto
            where TAdditionalInfoDto : AdditionalInfoDto;
    }
}