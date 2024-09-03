// <copyright file="SaveSafeReturn.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Service
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// SaveSafeReturn.
    /// </summary>
    /// <typeparam name="TOtherDto">The type of the other dto.</typeparam>
    public class SaveSafeReturn<TOtherDto>
    {
        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The dtos saved.
        /// </summary>
        public List<TOtherDto> DtosSaved { get; set; }

        /// <summary>
        /// Gets or sets the aggregate exception.
        /// </summary>
        /// <value>
        /// The aggregate exception.
        /// </value>
        public AggregateException AggregateException { get; set; }
    }
}
