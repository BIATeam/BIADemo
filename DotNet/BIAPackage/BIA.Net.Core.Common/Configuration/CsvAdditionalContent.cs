// <copyright file="CsvAdditionalContent.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The additional content in CSV generated files.
    /// </summary>
    public class CsvAdditionalContent
    {
        /// <summary>
        /// List of lines of text to display in the header of the CSV files generated.
        /// </summary>
        public List<string> Headers { get; set; }

        /// <summary>
        /// List of lines of text to display in the footer of the CSV files generated.
        /// </summary>
        public List<string> Footers { get; set; }
    }
}