// BIADemo only
// <copyright file="PlaneType.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Plane.Entities
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The plane entity.
    /// </summary>
    public class PlaneType : VersionedTable, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        public DateTime? CertificationDate { get; set; }
    }
}