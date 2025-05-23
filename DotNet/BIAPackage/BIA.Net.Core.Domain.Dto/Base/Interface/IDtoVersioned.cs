// <copyright file="IDtoVersioned.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base.Interface
{
    /// <summary>
    /// The base class for DTO.
    /// </summary>
    public interface IDtoVersioned
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
