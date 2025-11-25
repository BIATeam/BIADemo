// <copyright file="PagingFilterFormatDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using BIA.Net.Core.Domain.Dto.Base.Interface;

    /// <summary>
    /// The DTO used for lazy loading with filters, sort and paging.
    /// </summary>
    public class PagingFilterFormatDto : PagingFilterFormatDto<object>
    {
    }

    /// <summary>
    /// The DTO used for lazy loading with filters, sort and paging.
    /// </summary>
    /// <typeparam name="TAdvancedFilter">The type for advanced filter.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public class PagingFilterFormatDto<TAdvancedFilter> : IPagingFilterFormatDto<TAdvancedFilter>
#pragma warning restore SA1402 // File may only contain a single type
    {
        /// <inheritdoc/>
        public string[] ParentIds { get; set; }

        /// <inheritdoc/>
        public Dictionary<string, string> Columns { get; set; }

        /// <inheritdoc/>
        public TAdvancedFilter AdvancedFilter { get; set; }

        /// <inheritdoc/>
        public int? First { get; set; }

        /// <inheritdoc/>
        public int? Rows { get; set; }

        /// <inheritdoc/>
        public string SortField { get; set; }

        /// <inheritdoc/>
        public int? SortOrder { get; set; }

        /// <inheritdoc/>
        public List<SortMeta> MultiSortMeta { get; set; }

        /// <inheritdoc/>
        public Dictionary<string, JsonElement> Filters { get; set; }

        /// <inheritdoc/>
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
            trace.AppendFormat("MultiSortMeta: {0}, ", this.MultiSortMeta);
            trace.AppendFormat("filters: {0}, ", this.Filters);
            trace.AppendFormat("globalFilter: {0}]", this.GlobalFilter);
            return trace.ToString();
        }
    }
}