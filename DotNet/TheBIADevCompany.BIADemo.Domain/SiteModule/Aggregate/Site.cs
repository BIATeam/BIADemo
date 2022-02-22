// <copyright file="Site.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The site entity.
    /// </summary>
    public class Site : Team
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the collection of view site.
        /// </summary>
        public ICollection<ViewSite> ViewSites { get; set; }

        /// <summary>
        /// Add row version timestamp in table Site.
        /// </summary>
        [Timestamp]
        [Column("RowVersion")]
        public byte[] RowVersionSite { get; set; }
    }
}