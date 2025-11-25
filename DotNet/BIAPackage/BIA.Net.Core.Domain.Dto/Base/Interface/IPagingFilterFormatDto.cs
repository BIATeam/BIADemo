// <copyright file="IPagingFilterFormatDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base.Interface
{
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// Generic interface for <see cref="IPagingFilterFormatDto"/> implementation.
    /// </summary>
    /// <typeparam name="TAdvancedFilter">Generic advanced filter type.</typeparam>
    public interface IPagingFilterFormatDto<TAdvancedFilter> : IPagingFilterFormatDto
    {
        /// <summary>
        /// Name of the property and her translation for file export.
        /// </summary>
        public TAdvancedFilter AdvancedFilter { get; set; }
    }

    /// <summary>
    /// Interface for <see cref="IPagingFilterFormatDto"/> implementation.
    /// </summary>
    public interface IPagingFilterFormatDto
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
        /// Gets or sets the multi sort metas.
        /// </summary>
        public List<SortMeta> MultiSortMeta { get; set; }

        /// <summary>
        /// Gets or sets the list of columns filters.
        /// </summary>
        public Dictionary<string, JsonElement> Filters { get; set; }

        /// <summary>
        /// Gets or sets the global filter.
        /// </summary>
        public object GlobalFilter { get; set; }
    }
}
