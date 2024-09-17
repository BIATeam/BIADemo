// <copyright file="DataBaseHandlerChangedData.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a changed data handle by a database handler.
    /// </summary>
    public class DataBaseHandlerChangedData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseHandlerChangedData"/> class.
        /// </summary>
        /// <param name="changeType"><see cref="DatabaseHandlerChangeType"/>.</param>
        /// <param name="previousData">Previous data value if any.</param>
        /// <param name="currentData">Current data value if any.</param>
        public DataBaseHandlerChangedData(DatabaseHandlerChangeType changeType, Dictionary<string, object> previousData = null, Dictionary<string, object> currentData = null)
        {
            this.ChangeType = changeType;
            this.PreviousData = previousData;
            this.CurrentData = currentData;
        }

        /// <summary>
        /// The change type.
        /// </summary>
        public DatabaseHandlerChangeType ChangeType { get; }

        /// <summary>
        /// The previous data.
        /// </summary>
        public Dictionary<string, object> PreviousData { get; }

        /// <summary>
        /// The current data.
        /// </summary>
        public Dictionary<string, object> CurrentData { get; }
    }
}
