// <copyright file="PostgreSqlTemporaryTableProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System.Data.Common;
    using Npgsql;

    /// <summary>
    /// Implementation of temporary table management for PostgreSQL.
    /// </summary>
    internal class PostgreSqlTemporaryTableProvider : TemporaryTableProvider
    {
        /// <inheritdoc/>
        protected override char IdentifierQuoteOpen => '"';

        /// <inheritdoc/>
        protected override char IdentifierQuoteClose => '"';

        /// <inheritdoc/>
        protected override string TempTablePrefix => string.Empty;

        /// <inheritdoc/>
        protected override bool RequiresTemporaryKeyword => true;

        /// <inheritdoc/>
        protected override bool SupportsIfExists => true;

        /// <inheritdoc/>
        protected override DbParameter CreateParameter(string parameterName, object value)
        {
            return new NpgsqlParameter(parameterName, value);
        }
    }
}
