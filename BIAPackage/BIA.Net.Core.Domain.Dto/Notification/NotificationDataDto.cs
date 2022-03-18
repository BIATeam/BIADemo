// <copyright file="NotificationDataDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Notification
{
    /// <summary>
    /// The DTO used for notification data.
    /// </summary>
    public class NotificationDataDto
    {
        /// <summary>
        /// Gets or sets the Angular route parameters.
        /// </summary>
        public string[] Route { get; set; }

        public string Display { get; set; }

        public NotificationDataDto()
        {
            this.Display = "bia.action";
        }
    }
}