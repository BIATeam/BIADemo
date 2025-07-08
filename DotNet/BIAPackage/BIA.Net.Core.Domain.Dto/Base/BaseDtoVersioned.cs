// <copyright file="BaseDtoVersioned.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using BIA.Net.Core.Domain.Dto.Base.Interface;

    /// <summary>
    /// The base class for DTO.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public class BaseDtoVersioned<TKey> : BaseDto<TKey>, IDtoVersioned
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public string RowVersion { get; set; }
    }
}