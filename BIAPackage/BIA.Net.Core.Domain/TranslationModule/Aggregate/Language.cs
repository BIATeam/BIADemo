// <copyright file="Language.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.TranslationModule.Aggregate
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class Language : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string Name { get; set; }
    }
}