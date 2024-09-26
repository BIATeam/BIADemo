// <copyright file="FrontUserExceptionErrorMessageKey.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Enum
{
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
        /// Message key when errors occurs when modifying entity
        /// </summary>
        ModifyEntity,

        /// <summary>
        /// Message key when errors occurs when deleting entity
        /// </summary>
        DeleteEntity,

        /// <summary>
        /// Message key when errors occurs when validation failed for an entity
        /// </summary>
        ValidationEntity,

        /// <summary>
        /// Message key when errors occurs in database due to violation of FK constraint
        /// </summary>
        DatabaseForeignKeyConstraint,

        /// <summary>
        /// Message key when errors occurs in database due to violation of unique constraint
        /// </summary>
        DatabaseUniqueConstraint,

        /// <summary>
        /// Message key when errors occurs in database due to duplicate key violation
        /// </summary>
        DatabaseDuplicateKey,

        /// <summary>
        /// Message key when errors occurs in database due to unable to insert NULL value
        /// </summary>
        DatabaseNullValueInsert,

        /// <summary>
        /// Message key when errors occurs in database due to required object not found
        /// </summary>
        DatabaseObjectNotFound,

        /// <summary>
        /// Message key when errors occurs in database due to login user failure
        /// </summary>
        DatabaseLoginUser,

        /// <summary>
        /// Message key when errors occurs in database due to opening database failure
        /// </summary>
        DatabaseOpen,
    }
}
