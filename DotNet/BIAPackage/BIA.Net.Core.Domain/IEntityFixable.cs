// <copyright file="IEntityFixable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;

    /// <summary>
    /// Inrerface of a fixable entity.
    /// </summary>
    /// <typeparam name="TKey">Key type of the entity.</typeparam>
    public interface IEntityFixable<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the is fixed.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Gets or sets the fixed date.
        /// </summary>
        public DateTime? FixedDate { get; set; }
    }
}
