// <copyright file="ILazyLoadDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base.Interface
{
    using System.Collections.Generic;
    using System.Text.Json;

    public interface ILazyLoadDto
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