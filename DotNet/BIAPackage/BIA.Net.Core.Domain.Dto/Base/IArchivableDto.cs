// <copyright file="IArchivableDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System;

    /// <summary>
    /// The base class for DTO.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IArchivableDto : IFixableDto
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
