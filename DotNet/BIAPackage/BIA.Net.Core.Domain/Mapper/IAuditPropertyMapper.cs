// <copyright file="IAuditPropertyMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;

    /// <summary>
    /// Interface for audit property mappers.
    /// </summary>
    public interface IAuditPropertyMapper
    {
        /// <summary>
        /// Type of the linked entity to the entity.
        /// </summary>
        Type LinkedEntityType { get; }

        /// <summary>
        /// Property name into entity that have reference to the linked audit entity.
        /// </summary>
        string EntityPropertyName { get; }

        /// <summary>
        /// Property name into entity used as reference identifier to the linked audit entity.
        /// </summary>
        string EntityPropertyIdentifierName { get; }

        /// <summary>
        /// Property name of the linked entity used as display.
        /// </summary>
        string LinkedEntityPropertyDisplayName { get; }
    }
}