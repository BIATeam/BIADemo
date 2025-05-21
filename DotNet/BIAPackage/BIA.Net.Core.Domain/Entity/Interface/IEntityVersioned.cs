// <copyright file="IEntityVersioned.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity.Interface
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Inrerface of a fixable entity.
    /// </summary>
    /// <typeparam name="TKey">Key type of the entity.</typeparam>
    public interface IEntityVersioned
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
