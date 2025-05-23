// <copyright file="BaseDtoVersionedTeamFixableArchivable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Base
{
    using System;
    using BIA.Net.Core.Domain.Dto.Base.Interface;

    /// <summary>
    /// The base class for DTO.
    /// </summary>
    public class BaseDtoVersionedTeamFixableArchivable : BaseDtoVersionedTeamFixable, IDtoArchivable
    {
        /// <summary>
        /// Gets or sets the is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the archived date.
        /// </summary>
        public DateTime? ArchivedDate { get; set; }
    }
}