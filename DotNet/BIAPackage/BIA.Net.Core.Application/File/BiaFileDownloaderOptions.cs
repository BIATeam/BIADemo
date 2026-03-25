// <copyright file="BiaFileDownloaderOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.File
{
    /// <summary>
    /// Options for the <see cref="BiaFileDownloaderService{TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto}"/>.
    /// Must be inherited by project-specific options classes to allow for future extension without breaking changes.
    /// </summary>
    public abstract class BiaFileDownloaderOptions
    {
        /// <summary>
        /// Gets or sets the ID of the French language.
        /// </summary>
        public int FrenchLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the English language.
        /// </summary>
        public int EnglishLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Spanish language.
        /// </summary>
        public int SpanishLanguageId { get; set; }
    }
}
