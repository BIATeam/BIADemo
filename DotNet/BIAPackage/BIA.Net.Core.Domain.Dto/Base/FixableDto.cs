// <copyright file="FixableDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System;

    /// <summary>
    /// Represents the DTO for fixable entity.
    /// </summary>
    /// <typeparam name="TKey">Key type of the DTO.</typeparam>
    public class FixableDto<TKey> : BaseDto<TKey>
    {
        /// <summary>
        /// Gets or sets the IsFixed.
        /// </summary>
        public bool IsFixed { get; set; }
    }
}
