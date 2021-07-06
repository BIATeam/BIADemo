// <copyright file="FileFilterDto.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The Dto use to generate a file (csv for example).
    /// </summary>
    public class FileFiltersDto : LazyLoadDto
    {
        /// <summary>
        /// Name of the property and her translation.
        /// </summary>
        public Dictionary<string, string> Columns { get; set; }
    }
}
