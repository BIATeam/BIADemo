// <copyright file="EntityHistoricalEntryModificationDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Historic
{
    /// <summary>
    /// Entity historical entry modification DTO.
    /// </summary>
    public class EntityHistoricalEntryModificationDto
    {
        /// <summary>
        /// Gets or sets the PropertyName.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the OldValue.
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the NewValue.
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Indicates if this modification concerns a property linked to another entity.
        /// </summary>
        public bool IsLinkedProperty { get; set; }
    }
}
