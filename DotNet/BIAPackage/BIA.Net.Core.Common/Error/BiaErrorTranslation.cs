// <copyright file="BiaErrorTranslation.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Error
{
    /// <summary>
    /// Class that contains the translation for an error on a language.
    /// </summary>
    public sealed class BiaErrorTranslation
    {
        /// <summary>
        /// Gets or sets the error id.
        /// </summary>
        public int ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the label translated.
        /// </summary>
        public string Label { get; set; }
    }
}
