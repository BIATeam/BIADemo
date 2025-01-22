// <copyright file="VersionedDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The versioned dto class.
    /// </summary>
    /// <typeparam name="TKey">Key type of DTO.</typeparam>
    public class VersionedDto<TKey> : BaseDto<TKey>
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
