﻿// <copyright file="PagingFilterFormatDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System.Collections.Generic;
    using System.Text;

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
    public class PagingFilterFormatDto<TAdvancedFilter> : LazyLoadDto
#pragma warning restore SA1402 // File may only contain a single type
    {
        /// <summary>
        /// Gets or sets the parent ids.
        /// </summary>
        public string[] ParentIds { get; set; }

        /// <summary>
        /// Name of the property and her translation for file export.
        /// </summary>
        public Dictionary<string, string> Columns { get; set; }

        /// <summary>
        /// Name of the property and her translation for file export.
        /// </summary>
        public TAdvancedFilter AdvancedFilter { get; set; }

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