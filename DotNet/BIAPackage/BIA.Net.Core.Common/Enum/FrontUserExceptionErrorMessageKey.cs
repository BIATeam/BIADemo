namespace BIA.Net.Core.Common.Enum
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Enum of availables front user exception error message keys.
    /// </summary>
    public enum FrontUserExceptionErrorMessageKey
    {
        /// <summary>
        /// Message key when error is unkwown
        /// </summary>
        Unknown,

        /// <summary>
        /// Message key when errors occurs when adding entity
        /// </summary>
        AddEntity,

        /// <summary>
        /// Message key when errors occurs when editing entity
        /// </summary>
        EditEntity,

        /// <summary>
        /// Message key when errors occurs when deleting entity
        /// </summary>
        DeleteEntity,

        /// <summary>
        /// Message key when errors occurs when reading entity
        /// </summary>
        ReadEntity,

        /// <summary>
        /// Message key when errors occurs when validation failed for an entity
        /// </summary>
        ValidationEntity,

        /// <summary>
        /// Message key when errors occurs due to violation of FK constraint
        /// </summary>
        DatabaseForeignKeyConstraint,
    }
}
