// <copyright file="IDbContextDatabase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System;
    using System.Data.Common;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface DbContext Database.
    /// </summary>
    public interface IDbContextDatabase
    {
        /// <summary>
        /// Gets the underlying ADO.NET <see cref="DbConnection" /> for this <see cref="DbContext" />.
        /// </summary>
        /// <returns>The <see cref="DbConnection" />.</returns>
        DbConnection GetDbConnection();

        /// <summary>
        /// Run SQL scripts stored in an assembly embedded resources folder.
        /// </summary>
        /// <param name="assembly">Assembly that contains the embedded resources.</param>
        /// <param name="relativeResourcesFolderPath">Relative path to SQL scripts folder in embedded resources.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task RunScriptsFromAssemblyEmbeddedResourcesFolder(Assembly assembly, string relativeResourcesFolderPath);

        /// <summary>
        ///  Sets the timeout (in seconds) to use for commands executed with this <see cref="DbContext" />.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        void SetCommandTimeout(TimeSpan timeout);

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// </summary>
        void Migrate();
    }
}
