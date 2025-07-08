// BIADemo only
// <copyright file="Part.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The Part Entity.
    /// </summary>
    public class Part : BaseEntityVersioned<int>
    {
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
