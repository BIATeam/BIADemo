// <copyright file="DatabaseHandlerChangeType.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    /// <summary>
    /// The change type of the database handler.
    /// </summary>
    public enum DatabaseHandlerChangeType
    {
        /// <summary>
        /// Added data
        /// </summary>
        Add,

        /// <summary>
        /// Deleted data
        /// </summary>
        Delete,

        /// <summary>
        /// Modified data
        /// </summary>
        Modify,
    }
}
