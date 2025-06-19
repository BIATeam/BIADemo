// <copyright file="BaseAdditionalInfoDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// Adtitionnal Information send to the front. Cannot be customized.
    /// </summary>
    public abstract class BaseAdditionalInfoDto
    {
        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        public ICollection<BaseDtoVersionedTeam> Teams { get; set; }
    }
}
