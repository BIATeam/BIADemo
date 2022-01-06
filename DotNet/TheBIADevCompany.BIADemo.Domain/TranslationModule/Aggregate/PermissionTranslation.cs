// <copyright file="PermissionTranslation.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate
{
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.TranslationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class PermissionTranslation : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        ///  Gets or sets the notification type.
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// Gets or sets the notification type id.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the label translated.
        /// </summary>
        public string Label { get; set; }
    }
}