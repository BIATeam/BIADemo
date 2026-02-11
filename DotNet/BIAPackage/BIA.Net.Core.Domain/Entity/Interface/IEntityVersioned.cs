// <copyright file="IEntityVersioned.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity.Interface
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Helpers;

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
        /// Gets or sets the row version (Postgre).
        /// </summary>
        public uint RowVersionXmin { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public string RowVersionString
        {
            get
            {
                var type = this.GetType();
                var rowVersionProperty = type.GetBiaRowVersionProperty(DbProvider.SqlServer) ?? type.GetProperty(nameof(this.RowVersion));

                if (rowVersionProperty != null)
                {
                    return rowVersionProperty.GetValue(this) is byte[] value ? Convert.ToBase64String(value) : null;
                }

                var rowVersionXminProperty = type.GetBiaRowVersionProperty(DbProvider.PostGreSql) ?? type.GetProperty(nameof(this.RowVersionXmin));
                if (rowVersionXminProperty != null)
                {
                    return rowVersionXminProperty.GetValue(this)?.ToString();
                }

                return null;
            }
        }
    }
}
