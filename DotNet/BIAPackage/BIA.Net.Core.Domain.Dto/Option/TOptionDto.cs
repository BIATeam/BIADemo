// <copyright file="TOptionDto.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Option
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The generic DTO used to represent an option.
    /// </summary>
    /// <typeparam name="TKey">type of the key.</typeparam>
    public class TOptionDto<TKey> : BaseDto<TKey>
    {
        /// <summary>
        /// Gets or sets the display.
        /// </summary>
        public string Display { get; set; }
    }
}
