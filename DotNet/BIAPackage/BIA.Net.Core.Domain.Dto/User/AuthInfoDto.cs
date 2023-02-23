// <copyright file="AuthInfoDTO.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace BIA.Net.Core.Domain.Dto.User
{
    public class AuthInfoDTO<TUserDataDto, TAdditionalInfoDto> 
        where TUserDataDto : UserDataDto 
        where TAdditionalInfoDto: AdditionalInfoDto
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
