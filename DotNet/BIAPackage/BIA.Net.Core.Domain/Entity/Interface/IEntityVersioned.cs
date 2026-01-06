// <copyright file="IEntityVersioned.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity.Interface
{
    using System;

    /// <summary>
    /// Inrerface of a fixable entity.
    /// </summary>
    /// <typeparam name="TKey">Key type of the entity.</typeparam>
    public interface IEntityVersioned
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public uint RowVersionXmin { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public string RowVersionString
        {
            get
            {
                if (this.RowVersion != default)
                {
                    return Convert.ToBase64String(this.RowVersion);
                }
                else if (this.RowVersionXmin != default)
                {
                    return this.RowVersionXmin.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
