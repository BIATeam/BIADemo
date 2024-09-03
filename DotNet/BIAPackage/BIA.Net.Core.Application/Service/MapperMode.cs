// <copyright file="MapperMode.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Service
{
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
