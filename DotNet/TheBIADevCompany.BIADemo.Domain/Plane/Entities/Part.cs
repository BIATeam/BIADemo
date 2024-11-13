// BIADemo only
// <copyright file="Part.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Plane.Entities
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The Part Entity.
    /// </summary>
    public class Part : VersionedTable, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// Gets or sets the family.
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public decimal Price { get; set; }
    }
}
