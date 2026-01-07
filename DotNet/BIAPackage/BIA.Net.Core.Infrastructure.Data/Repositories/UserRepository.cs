// <copyright file="UserRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides a repository for managing user entities, supporting operations such as retrieving user information by
    /// login. Inherits standard data access functionality for entities identified by an integer key.
    /// </summary>
    /// <typeparam name="TUserEntity">The type of user entity managed by the repository. Must inherit from <see cref="BaseEntityUser"/>.</typeparam>
    public sealed class UserRepository<TUserEntity> : TGenericRepositoryEF<TUserEntity, int>, IUserRepository<TUserEntity>
        where TUserEntity : BaseEntityUser
    {
#pragma warning disable CA2100
        private const double UserFullNameLoginCacheDurationInMinutes = 60;
        private const string UserFullNameLoginCacheKeyPrefix = "UserFullName_";
        private readonly IQueryableUnitOfWork unitOfWork;
        private readonly IBiaDistributedCache distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository{TUserEntity}"/> class using the specified unit of work and service.
        /// provider.
        /// </summary>
        /// <param name="unitOfWork">The unit of work that manages database transactions and provides access to queryable data sources. Cannot be
        /// null.</param>
        /// <param name="serviceProvider">The service provider used to resolve application services and dependencies required by the repository.
        /// Cannot be null.</param>
        public UserRepository(IQueryableUnitOfWork unitOfWork, IServiceProvider serviceProvider)
            : base(unitOfWork, serviceProvider)
        {
            this.unitOfWork = unitOfWork;
            this.distributedCache = serviceProvider.GetRequiredService<IBiaDistributedCache>();
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> GetUserFullNamesPerLogins(IEnumerable<string> logins)
        {
            var distinctLogins = logins?.Distinct().ToList();
            if (distinctLogins?.Count == 0)
            {
                return [];
            }

            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var loginsNotInCache = new List<string>();

            if (this.distributedCache != null)
            {
                foreach (var login in distinctLogins)
                {
                    var cacheKey = GetUserFullNameLoginCacheKey(login);
                    var cachedFullName = await this.distributedCache.Get<string>(cacheKey);
                    if (cachedFullName != null)
                    {
                        result[login] = cachedFullName;
                    }
                    else
                    {
                        loginsNotInCache.Add(login);
                    }
                }
            }
            else
            {
                loginsNotInCache = distinctLogins;
            }

            if (loginsNotInCache.Count == 0)
            {
                return result;
            }

            var userFullNamesPerLoginsFromDb = await this.GetUserFullNamesFromDatabaseAsync(loginsNotInCache);

            foreach (var userFullNamePerLogin in userFullNamesPerLoginsFromDb)
            {
                result[userFullNamePerLogin.Key] = userFullNamePerLogin.Value;
                if (this.distributedCache != null)
                {
                    var cacheKey = GetUserFullNameLoginCacheKey(userFullNamePerLogin.Key);
                    await this.distributedCache.Add(cacheKey, userFullNamePerLogin.Value, UserFullNameLoginCacheDurationInMinutes);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the cache key for a login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>The cache key.</returns>
        private static string GetUserFullNameLoginCacheKey(string login)
        {
            return $"{UserFullNameLoginCacheKeyPrefix}{login}";
        }

        /// <summary>
        /// Determines the database provider from the DbContext.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>The database provider.</returns>
        /// <exception cref="NotSupportedException">If the database provider is not supported.</exception>
        private static DbProvider GetDatabaseProvider(DbContext dbContext)
        {
            return dbContext.Database switch
            {
                var db when db.IsSqlServer() => DbProvider.SqlServer,
                var db when db.IsNpgsql() => DbProvider.PostGreSql,
                _ => throw new NotSupportedException($"Database provider {dbContext.Database.ProviderName} is not supported for this operation"),
            };
        }

        /// <summary>
        /// Creates and populates a temporary table with login values.
        /// Supports SQL Server and PostgreSQL.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="logins">The list of logins to insert.</param>
        /// <param name="loginColumnType">The SQL type of the login column.</param>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="connection">The database connection (must be open).</param>
        /// <returns>A completed task.</returns>
        private static async Task CreateAndPopulateLoginTempTableAsync(
            string tempTableName,
            List<string> logins,
            string loginColumnType,
            DbProvider dbProvider,
            System.Data.Common.DbConnection connection)
        {
            var createTableSql = dbProvider switch
            {
                DbProvider.SqlServer => $@"CREATE TABLE [#{tempTableName}] (
    [Login] {loginColumnType} NOT NULL PRIMARY KEY
);",
                DbProvider.PostGreSql => $@"CREATE TEMPORARY TABLE ""{tempTableName}"" (
    ""Login"" {loginColumnType} NOT NULL PRIMARY KEY
);",
                _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
            };

            using (var command = connection.CreateCommand())
            {
                command.CommandText = createTableSql;
                await command.ExecuteNonQueryAsync();
            }

            if (logins.Count > 0)
            {
                var insertBatches = logins.Select((login, index) => new { login, index })
                    .GroupBy(x => x.index / 1000)
                    .ToList();

                foreach (var batch in insertBatches)
                {
                    var batchLogins = batch.Select(x => x.login).ToList();
                    var valuesList = string.Join(",", batchLogins.Select((_, i) => $"(@Login{i})"));

                    var insertSql = dbProvider switch
                    {
                        DbProvider.SqlServer => $@"INSERT INTO [#{tempTableName}] ([Login]) VALUES {valuesList}",
                        DbProvider.PostGreSql => $@"INSERT INTO ""{tempTableName}"" (""Login"") VALUES {valuesList}",
                        _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
                    };

                    using var command = connection.CreateCommand();
                    command.CommandText = insertSql;

                    if (dbProvider == DbProvider.SqlServer)
                    {
                        foreach (var (login, i) in batchLogins.Select((l, idx) => (l, idx)))
                        {
                            command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter($"@Login{i}", login ?? (object)DBNull.Value));
                        }
                    }
                    else if (dbProvider == DbProvider.PostGreSql)
                    {
                        foreach (var (login, i) in batchLogins.Select((l, idx) => (l, idx)))
                        {
                            command.Parameters.Add(new Npgsql.NpgsqlParameter($"@Login{i}", login ?? (object)DBNull.Value));
                        }
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Retrieves user full names from the temporary table using raw SQL query.
        /// Uses INNER JOIN for better performance instead of EXISTS.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="tableName">The users table name from EF Core metadata.</param>
        /// <param name="loginColumnName">The login column name from EF Core metadata.</param>
        /// <param name="firstNameColumnName">The first name column name from EF Core metadata.</param>
        /// <param name="lastNameColumnName">The last name column name from EF Core metadata.</param>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="connection">The database connection (must be open).</param>
        /// <returns>A dictionary mapping logins to full names.</returns>
        private static async Task<Dictionary<string, string>> GetUserFullNamesFromLoginTempTableAsync(
            string tempTableName,
            string tableName,
            string loginColumnName,
            string firstNameColumnName,
            string lastNameColumnName,
            DbProvider dbProvider,
            System.Data.Common.DbConnection connection)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using var command = connection.CreateCommand();
            command.CommandText = dbProvider switch
            {
                DbProvider.SqlServer => $@"
SELECT u.[{loginColumnName}], u.[{firstNameColumnName}], u.[{lastNameColumnName}]
FROM [{tableName}] u
INNER JOIN [#{tempTableName}] t ON u.[{loginColumnName}] = t.[Login]",
                DbProvider.PostGreSql => $@"
SELECT u.""{loginColumnName}"", u.""{firstNameColumnName}"", u.""{lastNameColumnName}""
FROM ""{tableName}"" u
INNER JOIN ""{tempTableName}"" t ON u.""{loginColumnName}"" = t.""Login""",
                _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
            };

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var login = reader.GetString(0);
                var firstName = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
                var lastName = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
                result[login] = $"{lastName} {firstName}";
            }

            return result;
        }

        /// <summary>
        /// Drops the temporary table of logins.
        /// </summary>
        /// <param name="tempTableName">The name of the temporary table.</param>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="connection">The database connection (must be open).</param>
        /// <returns>A completed task.</returns>
        private static async Task DropLoginTempTableAsync(string tempTableName, DbProvider dbProvider, System.Data.Common.DbConnection connection)
        {
            try
            {
                var dropTableSql = dbProvider switch
                {
                    DbProvider.SqlServer => $"DROP TABLE [#{tempTableName}]",
                    DbProvider.PostGreSql => $@"DROP TABLE IF EXISTS ""{tempTableName}""",
                    _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
                };

                using var command = connection.CreateCommand();
                command.CommandText = dropTableSql;
                await command.ExecuteNonQueryAsync();
            }
            catch
            {
                // Suppress exceptions during cleanup
            }
        }

        /// <summary>
        /// Retrieves user full names from the database for the specified logins.
        /// Uses small lists for standard LINQ queries and large lists for temporary tables.
        /// </summary>
        /// <param name="logins">The logins to retrieve.</param>
        /// <returns>A dictionary mapping logins to full names.</returns>
        private async Task<Dictionary<string, string>> GetUserFullNamesFromDatabaseAsync(List<string> logins)
        {
            if (logins.Count < 100)
            {
                return await this.unitOfWork.RetrieveSet<TUserEntity>()
                    .Where(u => logins.Contains(u.Login))
                    .ToDictionaryAsync(
                        u => u.Login,
                        u => $"{u.LastName} {u.FirstName}",
                        StringComparer.OrdinalIgnoreCase);
            }

            if (this.unitOfWork is not DbContext dbContext)
            {
                throw new InvalidOperationException($"Unit of work must be a {nameof(DbContext)} instance");
            }

            var dbProvider = GetDatabaseProvider(dbContext);
            var entityType = this.unitOfWork.FindEntityType(typeof(TUserEntity));
            var loginProperty = entityType.FindProperty(nameof(BaseEntityUser.Login));
            var firstNameProperty = entityType.FindProperty(nameof(BaseEntityUser.FirstName));
            var lastNameProperty = entityType.FindProperty(nameof(BaseEntityUser.LastName));

            if (loginProperty == null || firstNameProperty == null || lastNameProperty == null)
            {
                throw new InvalidOperationException($"Required properties ({nameof(BaseEntityUser.Login)}, {nameof(BaseEntityUser.FirstName)}, {nameof(BaseEntityUser.LastName)}) not found on user entity");
            }

            var tableName = entityType.GetTableName();
            var loginColumnName = loginProperty.GetColumnName();
            var firstNameColumnName = firstNameProperty.GetColumnName();
            var lastNameColumnName = lastNameProperty.GetColumnName();
            var loginColumnType = loginProperty.GetColumnType();

            var tempTableName = $"TempLogins_{Guid.NewGuid():N}";

            var connection = dbContext.Database.GetDbConnection();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await CreateAndPopulateLoginTempTableAsync(tempTableName, logins, loginColumnType, dbProvider, connection);
                return await GetUserFullNamesFromLoginTempTableAsync(
                    tempTableName,
                    tableName,
                    loginColumnName,
                    firstNameColumnName,
                    lastNameColumnName,
                    dbProvider,
                    connection);
            }
            finally
            {
                await DropLoginTempTableAsync(tempTableName, dbProvider, connection);
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
#pragma warning restore CA2100
    }
}
