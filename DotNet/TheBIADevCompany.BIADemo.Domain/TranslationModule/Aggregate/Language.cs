// <copyright file="Role.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate
{
    using System.Collections.Generic;

    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class Language : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string Name { get; set; }
    }
}