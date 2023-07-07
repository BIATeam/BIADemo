// <copyright file="AuthInfoDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The authorization info contained in the JWT token.
    /// </summary>
    /// <typeparam name="TUserDataDto">The user data type.</typeparam>
    /// <typeparam name="TAdditionalInfoDto">The additionnal Info type.</typeparam>
    public class AuthInfoDto<TUserDataDto, TAdditionalInfoDto>
        where TUserDataDto : UserDataDto
        where TAdditionalInfoDto : AdditionalInfoDto
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public TokenDto<TUserDataDto> UncryptedToken { get; set; }

        /// <summary>
        /// Gets or sets the additionalInfos.
        /// </summary>
        public TAdditionalInfoDto AdditionalInfos { get; set; }
    }
}
