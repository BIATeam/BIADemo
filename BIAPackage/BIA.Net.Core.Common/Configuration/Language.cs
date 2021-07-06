// <copyright file="Language.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The Language configuration.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Gets or sets the country of the language.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code of the language.
        /// </summary>
        public string Code { get; set; }
    }
}