// <copyright file="MapperMode.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// Different type of mapper. This list is custommizable so it should be string. For this reason it is not an enum.
    /// </summary>
    public static class MapperMode
    {
        /// <summary>
        /// Use the mapper that map list of elements.
        /// </summary>
        public const string List = "List";

        /// <summary>
        /// Use the mapper that map item.
        /// </summary>
        public const string Item = "Item";
    }
}
