﻿// <copyright file="IJwtFactory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Authentication
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The interface defining the JWT factory.
    /// </summary>
    public interface IJwtFactory
    {
        /// <summary>
        /// Decrypt the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>The principal.</returns>
        ClaimsPrincipal GetPrincipalFromToken(string token, string secretKey);

        /// <summary>
        /// Generate the identity for a user.
        /// </summary>
        /// <typeparam name="TUserDataDto">The type of the user data dto.</typeparam>
        /// <param name="tokenDto">The token data.</param>
        /// <returns>
        /// The identity.
        /// </returns>
        ClaimsIdentity GenerateClaimsIdentity<TUserDataDto>(TokenDto<TUserDataDto> tokenDto)
            where TUserDataDto : BaseUserDataDto;

        /// <summary>
        /// Generate an encoded JWT.
        /// </summary>
        /// <param name="identity">The identity of the current user.</param>
        /// <typeparam name="TUserDataDto">The type of the user data dto.</typeparam>
        /// <returns>The encoded JWT as string.</returns>
        Task<string> GenerateEncodedTokenAsync<TUserDataDto>(ClaimsIdentity identity)
            where TUserDataDto : BaseUserDataDto;

        /// <summary>
        /// Generate a JWT.
        /// </summary>
        /// <typeparam name="TUserDataDto">Type of the user data. </typeparam>
        /// <typeparam name="TAdditionalInfoDto">Type of the additionnal infos.</typeparam>
        /// <param name="tokenDto">The token not uncrypted.</param>
        /// <param name="additionalInfos">Additionnal Info for front.</param>
        /// <param name="loginParam">login parameter.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<AuthInfoDto<TAdditionalInfoDto>> GenerateAuthInfoAsync<TUserDataDto, TAdditionalInfoDto>(TokenDto<TUserDataDto> tokenDto, TAdditionalInfoDto additionalInfos, LoginParamDto loginParam)
            where TUserDataDto : BaseUserDataDto
            where TAdditionalInfoDto : BaseAdditionalInfoDto;
    }
}