// <copyright file="AuthInfoDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The authorization info contained in the JWT token.
    /// </summary>
    /// <typeparam name="TAdditionalInfoDto">The additionnal Info type.</typeparam>
    public class AuthInfoDto<TAdditionalInfoDto>
        where TAdditionalInfoDto : BaseAdditionalInfoDto
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the additionalInfos.
        /// </summary>
        public TAdditionalInfoDto AdditionalInfos { get; set; }
    }
}
