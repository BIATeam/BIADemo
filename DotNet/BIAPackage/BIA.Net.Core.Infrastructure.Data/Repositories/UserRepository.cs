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
    using BIA.Net.Core.Infrastructure.Data.Helpers;
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
        public async Task<Dictionary<string, string>> GetUserFullNamesPerLoginsAsync(IEnumerable<string> logins)
        {
            var distinctLogins = logins?.Distinct();
            if (distinctLogins?.Any() != true)
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
                loginsNotInCache.AddRange(distinctLogins);
            }

            if (loginsNotInCache.Count == 0)
            {
                return result;
            }

            var userFullNamesPerLoginsFromDb = await this.GetUserFullNamesPerLoginsFromDatabaseAsync(loginsNotInCache);

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
        /// Retrieves user full names from the database for the specified logins.
        /// Uses small lists for standard LINQ queries and large lists for temporary tables.
        /// </summary>
        /// <param name="logins">The logins to retrieve.</param>
        /// <returns>A dictionary mapping logins to full names.</returns>
        private async Task<Dictionary<string, string>> GetUserFullNamesPerLoginsFromDatabaseAsync(List<string> logins)
        {
            if (logins.Count < 100)
            {
                return await this.GetUserFullNamesPerLoginsFromDatabaseLinqAsync(logins);
            }

            return await this.GetUserFullNamesPerLoginsFromDatabaseTemporaryTableAsync(logins);
        }

        /// <summary>
        /// Asynchronously retrieves the full names of users whose logins match the specified list.
        /// </summary>
        /// <param name="logins">A list of user login names to search for. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping each login
        /// name to the corresponding user's full name in the format "LastName FirstName". The dictionary is
        /// case-insensitive with respect to login names. If no users are found, the dictionary will be empty.</returns>
        private async Task<Dictionary<string, string>> GetUserFullNamesPerLoginsFromDatabaseLinqAsync(IReadOnlyList<string> logins)
        {
            return await this.unitOfWork.RetrieveSet<TUserEntity>()
                                .Where(u => logins.Contains(u.Login))
                                .ToDictionaryAsync(
                                    u => u.Login,
                                    u => $"{u.LastName} {u.FirstName}",
                                    StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retrieves the full names of users for the specified logins by querying the database using a temporary table.
        /// </summary>
        /// <param name="logins">A list of user login names for which to retrieve full names. Cannot be null.</param>
        /// <returns>A dictionary mapping each login name to the corresponding user's full name. If a login is not found, it will
        /// not be included in the dictionary.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the unit of work is not a DbContext instance, or if required user entity properties are missing.</exception>
        /// <exception cref="NotSupportedException">Thrown if the current database provider is not supported.</exception>
        private async Task<Dictionary<string, string>> GetUserFullNamesPerLoginsFromDatabaseTemporaryTableAsync(IReadOnlyList<string> logins)
        {
            if (this.unitOfWork is not DbContext dbContext)
            {
                throw new InvalidOperationException($"Unit of work must be a {nameof(DbContext)} instance");
            }

            var dbProvider = this.unitOfWork.GetDatabaseProviderEnum();
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

                const string temporaryTableColumnName = "Login";
                await TemporaryTableHelper.CreateAndPopulateTemporaryTableAsync(
                    tempTableName,
                    logins,
                    temporaryTableColumnName,
                    loginColumnType,
                    dbProvider,
                    connection);

                var selectQuery = dbProvider switch
                {
                    DbProvider.SqlServer => $@"
SELECT u.[{loginColumnName}], u.[{firstNameColumnName}], u.[{lastNameColumnName}]
FROM [{tableName}] u
INNER JOIN [#{tempTableName}] t ON u.[{loginColumnName}] = t.[{temporaryTableColumnName}]",
                    DbProvider.PostGreSql => $@"
SELECT u.""{loginColumnName}"", u.""{firstNameColumnName}"", u.""{lastNameColumnName}""
FROM ""{tableName}"" u
INNER JOIN ""{tempTableName}"" t ON u.""{loginColumnName}"" = t.""{temporaryTableColumnName}""",
                    _ => throw new NotSupportedException($"Database provider {dbProvider} is not supported"),
                };

                var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var resultList = await TemporaryTableHelper.GetDataFromTemporaryTableAsync(
                    selectQuery,
                    async reader =>
                    {
                        var login = reader.GetString(0);
                        var firstName = await reader.IsDBNullAsync(1) ? string.Empty : reader.GetString(1);
                        var lastName = await reader.IsDBNullAsync(2) ? string.Empty : reader.GetString(2);
                        return (login, FullName: $"{lastName} {firstName}");
                    },
                    connection);

                foreach (var (login, fullName) in resultList)
                {
                    result[login] = fullName;
                }

                return result;
            }
            finally
            {
                await TemporaryTableHelper.DropTemporaryTableAsync(tempTableName, dbProvider, connection);
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
        }
#pragma warning restore CA2100
    }
}
