// <copyright file="IMockEntityFramework.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Test.Data
{
    using BIA.Net.Core.Infrastructure.Data;

    /// <summary>
    /// Interface for database mock.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the database context.</typeparam>
    public interface IMockEntityFramework<TDbContext>
        where TDbContext : IQueryableUnitOfWork
    {
        /// <summary>
        /// Get the database context.
        /// It gives access to the available tables and can be used to add data in the database.
        /// </summary>
        /// <returns>The database context.</returns>
        TDbContext GetDbContext();

        /// <summary>
        /// Add default data in the database.
        /// </summary>
        void InitDefaultData();
    }
}
