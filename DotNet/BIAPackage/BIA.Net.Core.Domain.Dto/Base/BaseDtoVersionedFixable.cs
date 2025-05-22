// <copyright file="BaseDtoVersionedFixable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System;
    using BIA.Net.Core.Domain.Dto.Base.Interface;

    /// <summary>
    /// The base class for DTO.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public class BaseDtoVersionedFixable<TKey> : BaseDtoVersioned<TKey>, IDtoFixable
    {
        /// <summary>
        /// Gets or sets the IsFixed.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Gets or sets the list of connecting airports.
        /// </summary>
        public DateTime? FixedDate { get; set; }
    }
}