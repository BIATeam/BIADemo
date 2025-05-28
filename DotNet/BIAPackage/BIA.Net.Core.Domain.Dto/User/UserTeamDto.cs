// <copyright file="UserTeamDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    /// <summary>
    /// DTO of an user's team.
    /// </summary>
    public class UserTeamDto
    {
        /// <summary>
        /// The team title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The team type id.
        /// </summary>
        public int TeamTypeId { get; set; }
    }
}
