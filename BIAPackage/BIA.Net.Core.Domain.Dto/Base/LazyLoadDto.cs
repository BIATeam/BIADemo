// <copyright file="LazyLoadDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The DTO used for lazy loading with filters, sort and paging.
    /// </summary>
    public class LazyLoadDto
    {
        /// <summary>
        /// Gets or sets the number of the first element to return.
        /// </summary>
        public int? First { get; set; }

        /// <summary>
        /// Gets or sets the number or rows to return.
        /// </summary>
        public int? Rows { get; set; }

        /// <summary>
        /// Gets or sets the sort field.
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// Gets or sets the sort order to indicate if it's in ascending.
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the list of columns filters.
        /// </summary>
        public Dictionary<string, Dictionary<string, object>> Filters { get; set; }

        /// <summary>
        /// Gets or sets the global filter.
        /// </summary>
        public object GlobalFilter { get; set; }

        /// <summary>
        /// Returns a string that represents of the current object.
        /// </summary>
        /// <returns>A formatted string of the object's values.</returns>
        public override string ToString()
        {
            var trace = new StringBuilder("record:[");
            trace.AppendFormat("first: {0}, rows: {1}, ", this.First, this.Rows);
            trace.AppendFormat("sortField: {0}, sortOrder: {1}, ", this.SortField, this.SortOrder);
            trace.AppendFormat("filters: {0}, ", this.Filters);
            trace.AppendFormat("globalFilter: {0}]", this.GlobalFilter);
            return trace.ToString();
        }
    }
}