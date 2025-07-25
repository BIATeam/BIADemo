// <copyright file="RoleTranslation.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Translation.Entities
{
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class RoleTranslation : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        ///  Gets or sets the role.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the label translated.
        /// </summary>
        public string Label { get; set; }
    }
}