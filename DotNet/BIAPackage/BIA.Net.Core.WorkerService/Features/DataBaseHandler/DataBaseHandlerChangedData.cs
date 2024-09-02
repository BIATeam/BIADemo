namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The change type of the database handler changed data
    /// </summary>
    public enum ChangeType
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

    /// <summary>
    /// Represents a changed data handle by a database handler.
    /// </summary>
    public class DataBaseHandlerChangedData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseHandlerChangedData"/> class.
        /// </summary>
        /// <param name="changeType"><see cref="ChangeType"/> of data.</param>
        /// <param name="previousData">Previous data value if any.</param>
        /// <param name="currentData">Current data value if any.</param>
        public DataBaseHandlerChangedData(ChangeType changeType, Dictionary<string, object> previousData = null, Dictionary<string, object> currentData = null)
        {
            this.ChangeType = changeType;
            this.PreviousData = previousData;
            this.CurrentData = currentData;
        }

        /// <summary>
        /// The change type.
        /// </summary>
        public ChangeType ChangeType { get; }

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
