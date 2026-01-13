// <copyright file="EntityHistoricalEntryDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Historic
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// Entity historical entry DTO.
    /// </summary>
    public class EntityHistoricalEntryDto
    {
        /// <summary>
        /// Gets or sets the EntryModifications.
        /// </summary>
        public List<EntityHistoricalEntryModificationDto> EntryModifications { get; set; } = [];

        /// <summary>
        /// Gets or sets the EntryType.
        /// </summary>
        public EntityHistoricEntryType EntryType { get; set; }

        /// <summary>
        /// Gets or sets the EntryDateTime.
        /// </summary>
        public DateTime EntryDateTime { get; set; }

        /// <summary>
        /// Gets or sets the EntryUser.
        /// </summary>
        public string EntryUser { get; set; }
    }
}
