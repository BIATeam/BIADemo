// BIADemo only
// <copyright file="Country.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Entities
{
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The Country entity.
    /// </summary>
    public class Country : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
