﻿// <copyright file="LoginParamDto.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    /// <summary>
    /// LoginParamDto.
    /// </summary>
    public class LoginParamDto
    {
        /// <summary>
        /// Gets or sets is the current teams logged.
        /// </summary>
        public CurrentTeamDto[] CurrentTeamLogins { get; set; }

        /// <summary>
        /// Gets or sets is the teams config.
        /// </summary>
        public TeamConfigDto[] TeamsConfig { get; set; }

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
    }
}
