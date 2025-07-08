// BIADemo only
// <copyright file="PlaneType.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using System;
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The plane entity.
    /// </summary>
    public class PlaneType : BaseEntityVersioned<int>
    {
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