// <copyright file="LoginParamDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Immutable;

    /// <summary>
    /// LoginParamDto.
    /// </summary>
    /// <typeparam name="TTeamConfigDto">Team config dto type.</typeparam>
    public class LoginParamDto
    {
        /// <summary>
        /// Gets or sets is the current teams logged.
        /// </summary>
        public CurrentTeamDto[] CurrentTeamLogins { get; set; }

        /// <summary>
        /// Gets or sets if it required a light token (only permition taged light token).
        /// </summary>
        public bool LightToken { get; set; }

        /// <summary>
        /// Gets or sets if it required a Only gloabal right and not fine grained rights.
        /// </summary>
        public bool FineGrainedPermission { get; set; }

        /// <summary>
        /// Gets or sets if it required aditionnal users (from database or ad and user profile) info in the token.
        /// </summary>
        public bool AdditionalInfos { get; set; }

        /// <summary>
        /// Inidcates wheither is ifrst login or not.
        /// </summary>
        public bool IsFirstLogin { get; set; }

        /// <summary>
        /// User to copy rights and roles from.
        /// </summary>
        public string BaseUserIdentity { get; set; }
    }
}
