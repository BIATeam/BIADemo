// <copyright file="Site.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Site.Entities
{
    using BIA.Net.Core.Domain.User.Entities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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