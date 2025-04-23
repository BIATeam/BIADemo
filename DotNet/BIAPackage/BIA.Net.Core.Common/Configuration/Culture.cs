// <copyright file="Culture.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The Language configuration.
    /// </summary>
    public class Culture
    {
        /// <summary>
        /// Gets or sets the Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the code culture.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the codes accepted to use this culture.
        /// </summary>
        public string[] AcceptedCodes { get; set; }

        /// <summary>
        /// Gets or sets the countries that use by default the culture.
        /// </summary>
        public string[] IsDefaultForCountryCodes { get; set; }

        /// <summary>
        /// Gets or sets the Date Format.
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the Time Format .
        /// </summary>
        public string TimeFormat { get; set; }

        /// <summary>
        /// Gets or sets the Time Format with second.
        /// </summary>
        public string TimeFormatSec { get; set; }

        /// <summary>
        /// Gets or sets the language Code.
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the language Id in database.
        /// </summary>
        public int LanguageId { get; set; }
    }
}