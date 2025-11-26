// <copyright file="Language.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Translation.Entities
{
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The language entity.
    /// </summary>
    public class Language : BaseEntityVersioned<int>
    {
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