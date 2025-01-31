// <copyright file="IEntityArchivable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Archive
{
    /// <summary>
    /// Interface for archivable entity.
    /// </summary>
    /// <typeparam name="TKey">Key type of the entity.</typeparam>
    public interface IEntityArchivable<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the <see cref="Archive.ArchiveState"/>.
        /// </summary>
        public ArchiveState ArchiveState { get; set; }
    }
}
