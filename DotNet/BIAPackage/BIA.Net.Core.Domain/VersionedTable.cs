namespace BIA.Net.Core.Domain
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The versioned table class used to apply RowVersion on all table.
    /// </summary>
    public class VersionedTable
    {

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
