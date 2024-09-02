namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
