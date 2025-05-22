// <copyright file="IDtoArchivable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base.Interface
{
    using System;

    /// <summary>
    /// The base class for DTO.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IDtoArchivable : IDtoFixable
    {
        /// <summary>
        /// Gets or sets the is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the archived date.
        /// </summary>
        public DateTime? ArchivedDate { get; set; }
    }
}
