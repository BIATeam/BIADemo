// <copyright file="TeamLoginDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

using BIA.Net.Core.Common.Enum;

namespace BIA.Net.Core.Domain.Dto.User
{
    public class TeamLoginDto
    {
        public int TeamTypeId { get; set; }

        public int TeamId { get; set; }

        public RoleMode RoleMode { get; set; }

        public int[] RoleIds { get; set; }

        public bool UseDefaultRoles { get; set; }
    }
}