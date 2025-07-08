// <copyright file="IEntityFixable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity.Interface
{
    using System;

    /// <summary>
    /// Inrerface of a fixable entity.
    /// </summary>
    public interface IEntityFixable
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
