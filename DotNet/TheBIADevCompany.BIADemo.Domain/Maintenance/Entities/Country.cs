// BIADemo only
// <copyright file="Country.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Entities
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The Country entity.
    /// </summary>
    public class Country : VersionedTable, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
