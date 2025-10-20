// <copyright file="BiaErrorId.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Error
{
    /// <summary>
    /// Enum of BIA error ids.
    /// </summary>
    public enum BiaErrorId
    {
        /// <summary>
        /// Error id when error is unknown.
        /// </summary>
        Unknown = 1000,

        /// <summary>
        /// Error id when internal server error.
        /// </summary>
        InternalServerError,

        /// <summary>
        /// Error id when the member already exist and cannot be added.
        /// </summary>
        MemberAlreadyExists,

        /// <summary>
        /// Error id when errors occurs when adding entity.
        /// </summary>
        AddEntity,

        /// <summary>
        /// Error id when errors occurs when modifying entity.
        /// </summary>
        ModifyEntity,

        /// <summary>
        /// Error id when errors occurs when deleting entity.
        /// </summary>
        DeleteEntity,

        /// <summary>
        /// Error id when errors occurs when validation failed for an entity.
        /// </summary>
        ValidationEntity,

        /// <summary>
        /// Error id when errors occurs in database due to violation of FK constraint.
        /// </summary>
        DatabaseForeignKeyConstraint,

        /// <summary>
        /// Error id when errors occurs in database due to violation of unique constraint.
        /// </summary>
        DatabaseUniqueConstraint,

        /// <summary>
        /// Error id when errors occurs in database due to duplicate key violation.
        /// </summary>
        DatabaseDuplicateKey,

        /// <summary>
        /// Error id when errors occurs in database due to unable to insert NULL value.
        /// </summary>
        DatabaseNullValueInsert,

        /// <summary>
        /// Error id when errors occurs in database due to required object not found.
        /// </summary>
        DatabaseObjectNotFound,

        /// <summary>
        /// Error id when errors occurs in database due to login user failure.
        /// </summary>
        DatabaseLoginUser,

        /// <summary>
        /// Error id when errors occurs in database due to opening database failure.
        /// </summary>
        DatabaseOpen,
    }
}
