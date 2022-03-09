// <copyright file="Site.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The site entity.
    /// </summary>
    public class Site : Team
    {
        /// <summary>
        /// Add row version timestamp in table Site.
        /// </summary>
        [Timestamp]
        [Column("RowVersion")]
        public byte[] RowVersionSite { get; set; }
    }
}