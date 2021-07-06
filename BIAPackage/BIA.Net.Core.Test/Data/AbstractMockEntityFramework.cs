// <copyright file="AbstractMockEntityFramework.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Test.Data
{
    using BIA.Net.Core.Infrastructure.Data;

    /// <summary>
    /// Abstract class managing mock of the DB context.
    /// 
    /// Note: <see cref="InitDefaultData"/> has to be overridden in children classes in order to implement logic that is strongly coupled to the DbContext 
    /// (and thus specific to the project).
    /// It can be empty if you want to always add data manually in your tests, rather than do it automatically before each test through AbstractUnitTest.InitTestBase().
    /// </summary>
    public abstract class AbstractMockEntityFramework<TDbContext> : IMockEntityFramework<TDbContext>
        where TDbContext : class, IQueryableUnitOfWork
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly TDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMockEntityFramework{TDbContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The DB context.</param>
        protected AbstractMockEntityFramework(IQueryableUnitOfWork dbContext)
        {
            this.dbContext = dbContext as TDbContext;
        }

        /// <inheritdoc cref="IMockEntityFramework{TDbContext}.GetDbContext"/>
        /// <returns>The "in memory" database context.</returns>
        public TDbContext GetDbContext()
        {
            return this.dbContext;
        }

        /// <inheritdoc cref="IMockEntityFramework{TDbContext}.InitDefaultData"/>
        public abstract void InitDefaultData();
    }
}
