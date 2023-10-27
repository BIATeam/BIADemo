// <copyright file="OptionDefaultDto.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Option
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used to represent a airport.
    /// </summary>
    public class OptionDefaultDto : OptionDto
    {
        /// <summary>
        /// Gets or sets is the defaul value.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
