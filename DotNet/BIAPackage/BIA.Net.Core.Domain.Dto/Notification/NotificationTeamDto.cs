// <copyright file="NotificationTeamDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Notification
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for notification team.
    /// </summary>
    public class NotificationTeamDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        public OptionDto Team { get; set; }

        /// <summary>
        /// Gets or sets the type id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the roles, if any, to filter.
        /// Users will be notified only if they have one of these roles
        /// on the current team.
        /// </summary>
        public List<OptionDto> Roles { get; set; }
    }
}