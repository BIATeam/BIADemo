// <copyright file="IDtoFixable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base.Interface
{
    using System;

    /// <summary>
    /// The base class for DTO.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IDtoFixable
    {
        /// <summary>
        /// Gets or sets the IsFixed.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Gets or sets the IsFixed.
        /// </summary>
        public DateTime? FixedDate { get; set; }
    }
}
