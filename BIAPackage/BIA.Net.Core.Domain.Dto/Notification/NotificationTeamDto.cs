// <copyright file="NotificationTeamDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Notification
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used for notification team.
    /// </summary>
    public class NotificationTeamDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the type id.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Display { get; set; }

    }
}