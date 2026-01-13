// <copyright file="ILinkedAuditMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;

    /// <summary>
    /// Interface for linked audit mappers.
    /// </summary>
    public interface ILinkedAuditMapper
    {
        /// <summary>
        /// The linked entity type.
        /// </summary>
        Type LinkedAuditEntityType { get; }

        /// <summary>
        /// Property name into entity that have reference to the linked audit entity.
        /// </summary>
        string EntityPropertyName { get; }

        /// <summary>
        /// Property name into linked audit entity used as display.
        /// </summary>
        string LinkedAuditEntityDisplayPropertyName { get; }

        /// <summary>
        /// Property name into linked audit entity used as identifier to the entity.
        /// </summary>
        string LinkedAuditEntityIdentifierPropertyName { get; }
    }
}
