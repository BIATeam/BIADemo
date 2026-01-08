// <copyright file="SqlServerTemporaryTableProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers.TemporaryTable
{
    using System.Data.Common;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Implementation of temporary table management for SQL Server.
    /// </summary>
    internal class SqlServerTemporaryTableProvider : TemporaryTableProvider
    {
        /// <inheritdoc/>
        protected override char IdentifierQuoteOpen => '[';

        /// <inheritdoc/>
        protected override char IdentifierQuoteClose => ']';

        /// <inheritdoc/>
        protected override string TempTablePrefix => "#";

        /// <inheritdoc/>
        protected override bool RequiresTemporaryKeyword => false;

        /// <inheritdoc/>
        protected override bool SupportsIfExists => false;

        /// <inheritdoc/>
        protected override DbParameter CreateParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }
    }
}
