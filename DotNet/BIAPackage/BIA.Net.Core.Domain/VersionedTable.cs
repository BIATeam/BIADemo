﻿// <copyright file="VersionedTable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain
{
    using System.ComponentModel.DataAnnotations;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The versioned table class used to apply RowVersion on all table.
    /// </summary>
    public class VersionedTable : IEntityVersioned
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
