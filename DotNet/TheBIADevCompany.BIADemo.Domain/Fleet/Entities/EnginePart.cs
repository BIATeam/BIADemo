// BIADemo only
// <copyright file="EnginePart.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity to link engines and parts.
    /// </summary>
    public class EnginePart : VersionedTable
    {
        /// <summary>
        /// Gets or sets the Engine.
        /// </summary>
        public Engine Engine { get; set; }

        /// <summary>
        /// Gets or sets the EngineId.
        /// </summary>
        public int EngineId { get; set; }

        /// <summary>
        /// Gets or sets the Part.
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// Gets or sets the PartId.
        /// </summary>
        public int PartId { get; set; }
    }
}
