// <copyright file="BaseDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    /// <summary>
    /// The base class for DTO.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public abstract class BaseDto<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the state of the DTO regarding to the DB context.
        /// </summary>
        public DtoState DtoState { get; set; }
    }
}