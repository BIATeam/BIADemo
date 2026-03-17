// <copyright file="BiaHybridCache.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.DistCache.Entities;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Hybrid;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Manages hybrid cache operations with default expirations and key generation.
    /// </summary>
    public class BiaHybridCache : IBiaHybridCache
    {
        /// <summary>
        /// The cache key prefix used for all entries managed by this class.
        /// </summary>
        protected const string PrefixKey = "BiaHCache";

        /// <summary>
        /// The cache key prefix used for team identifiers.
        /// </summary>
        protected const string PrefixTeam = "Team:";

        /// <summary>
        /// The cache key prefix used for tags.
        /// </summary>
        protected const string PrefixTag = "Tag:";

        /// <summary>
        /// The hybrid cache instance.
        /// </summary>
        private readonly HybridCache cache;

        /// <summary>
        /// The default expiration for cache entries.
        /// </summary>
        private readonly TimeSpan? expiration;

        /// <summary>
        /// The default local cache expiration for cache entries.
        /// </summary>
        private readonly TimeSpan? localCacheExpiration;

        /// <summary>
        /// The read-only unit of work used for cache persistence.
        /// </summary>
        private readonly IQueryableUnitOfWorkNoTracking unitOfWork;

        /// <summary>
        /// The logger for cache operations.
        /// </summary>
        private readonly ILogger<BiaHybridCache> logger;

        /// <summary>
        /// If it equals true, distributed cache cleanup is active.
        /// </summary>
        private readonly bool isDistributedCacheActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaHybridCache"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public BiaHybridCache(
            HybridCache cache,
            IQueryableUnitOfWorkNoTracking unitOfWork,
            IConfiguration configuration,
            ILogger<BiaHybridCache> logger)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(
                    nameof(cache),
                    "HybridCache is not configured. Ensure .AddHybridCache() is called during startup.");
            }

            this.cache = cache;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            BiaNetSection biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(biaNetSection);

            this.expiration = ResolveExpiration(biaNetSection);
            this.localCacheExpiration = ResolveLocalCacheExpiration(biaNetSection);
            this.isDistributedCacheActive = ResolveDistributedCacheActive(biaNetSection);
        }

        /// <inheritdoc />
        public virtual async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            string prefixKey = $"{PrefixKey}|{key}";
            await this.cache.RemoveAsync(key: prefixKey, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task RemoveAllAsync(CancellationToken cancellationToken = default)
        {
            await this.cache.RemoveByTagAsync("*");
            await this.RemoveDistCacheEntriesAsync(filter: null, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task RemoveByTeamIdAsync(int teamId, CancellationToken cancellationToken = default)
        {
            if (teamId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(teamId));
            }

            string tagTeamId = $"{PrefixTeam}{teamId}";
            await this.cache.RemoveByTagAsync(tagTeamId);
            await this.RemoveDistCacheEntriesAsync(filter: x => x.Id.Contains(tagTeamId), cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task RemoveByTagAsync(string tag, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentException("Tag must not be empty.", nameof(tag));
            }

            await this.cache.RemoveByTagAsync(tag);
            await this.RemoveDistCacheEntriesAsync(filter: x => x.Id.Contains(PrefixTag + tag), cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task RemoveByTagAsync(List<string> tags, CancellationToken cancellationToken = default)
        {
            tags = tags ?? new List<string>();

            if (tags.Any())
            {
                await this.cache.RemoveByTagAsync(tags);
                await this.RemoveDistCacheEntriesAsync(filter: x => tags.Any(tag => x.Id.Contains(PrefixTag + tag)), cancellationToken: cancellationToken);
            }
        }

        /// <inheritdoc />
        public virtual async Task<T> GetOrCreateAsync<T>(
            Expression<Func<CancellationToken, ValueTask<T>>> factoryExpression,
            int teamId = 0,
            TimeSpan? expiration = null,
            TimeSpan? localCacheExpiration = null,
            List<string> tags = null,
            string key = null,
            CancellationToken cancellationToken = default)
        {
            if (factoryExpression == null)
            {
                throw new ArgumentNullException(nameof(factoryExpression));
            }

            if (teamId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(teamId));
            }

            tags = tags ?? new List<string>();

            CacheState cacheState = this.ResolveCacheState(expiration, localCacheExpiration);

            if (!cacheState.IsCacheActive && !cacheState.IsLocalCacheActive)
            {
                return await factoryExpression.Compile()(cancellationToken);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            string resolvedKey = key;

            if (string.IsNullOrWhiteSpace(resolvedKey))
            {
                resolvedKey = this.ResolveCacheKey(factoryExpression, tags, teamId);
            }

            HybridCacheEntryOptions resolvedOptions = new HybridCacheEntryOptions
            {
                Expiration = cacheState.IsCacheActive ? cacheState.Expiration : default,
                LocalCacheExpiration = cacheState.IsLocalCacheActive ? cacheState.LocalCacheExpiration : default,
                Flags = cacheState.Flags,
            };

            LogHelper.End(logger: this.logger, className: nameof(BiaHybridCache), $"{nameof(this.GetOrCreateAsync)} Init", stopwatch: stopwatch);

            this.logger.LogInformation("Key: {Key}", resolvedKey);

            stopwatch.Restart();

            T result = await this.cache.GetOrCreateAsync(
                key: resolvedKey,
                factory: factoryExpression.Compile(),
                options: resolvedOptions,
                tags: tags.Append($"{PrefixTeam}{teamId}").ToList(),
                cancellationToken: cancellationToken);

            LogHelper.End(logger: this.logger, className: nameof(BiaHybridCache), $"{nameof(this.GetOrCreateAsync)} Call", stopwatch: stopwatch);

            return result;
        }

        /// <summary>
        /// Resolves entry flags for hybrid cache based on enabled cache layers.
        /// </summary>
        /// <param name="isCacheActive">Whether distributed cache is active.</param>
        /// <param name="isLocalCacheActive">Whether local cache is active.</param>
        /// <returns>The resolved hybrid cache entry flags.</returns>
        protected static HybridCacheEntryFlags GetHybridCacheEntryFlags(bool isCacheActive, bool isLocalCacheActive)
        {
            if (isCacheActive && isLocalCacheActive)
            {
                return HybridCacheEntryFlags.None;
            }
            else if (isCacheActive)
            {
                return HybridCacheEntryFlags.DisableLocalCache;
            }
            else if (isLocalCacheActive)
            {
                return HybridCacheEntryFlags.DisableDistributedCache;
            }

            return HybridCacheEntryFlags.DisableDistributedCache | HybridCacheEntryFlags.DisableLocalCache;
        }

        /// <summary>
        /// Extracts invocation metadata from the provided factory expression.
        /// </summary>
        /// <typeparam name="T">The cached value type.</typeparam>
        /// <param name="factoryExpression">The factory expression.</param>
        /// <returns>The resolved invocation information.</returns>
        protected static CacheInvocationInfo GetInvocationInfo<T>(Expression<Func<CancellationToken, ValueTask<T>>> factoryExpression)
        {
            MethodCallExpression methodCall = ExtractMethodCall(factoryExpression.Body);
            if (methodCall == null)
            {
                throw new ArgumentException("Factory expression must be a direct method call.", nameof(factoryExpression));
            }

            string className = methodCall.Method.DeclaringType?.Name;
            string methodName = methodCall.Method.Name;

            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Class name could not be resolved from factory expression.", nameof(factoryExpression));
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("Method name could not be resolved from factory expression.", nameof(factoryExpression));
            }

            object filters = BuildFilters(methodCall, factoryExpression.Parameters[0]);

            return new CacheInvocationInfo(className, methodName, filters);
        }

        /// <summary>
        /// Finds the first method call expression from a complex expression tree.
        /// </summary>
        /// <param name="expression">The expression to analyze.</param>
        /// <returns>The first method call expression, or null if none is found.</returns>
        protected static MethodCallExpression ExtractMethodCall(Expression expression)
        {
            return FindMethodCall(expression);
        }

        /// <summary>
        /// Recursively walks an expression tree to locate a method call.
        /// </summary>
        /// <param name="expression">The expression to inspect.</param>
        /// <returns>The found method call expression, or null when absent.</returns>
        protected static MethodCallExpression FindMethodCall(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            if (expression is MethodCallExpression methodCall)
            {
                return methodCall;
            }

            if (expression is UnaryExpression unaryExpression)
            {
                return FindMethodCall(unaryExpression.Operand);
            }

            if (expression is NewExpression newExpression)
            {
                foreach (Expression argument in newExpression.Arguments)
                {
                    MethodCallExpression argumentMethodCall = FindMethodCall(argument);
                    if (argumentMethodCall != null)
                    {
                        return argumentMethodCall;
                    }
                }

                return null;
            }

            if (expression is InvocationExpression invocationExpression)
            {
                MethodCallExpression invokedMethodCall = FindMethodCall(invocationExpression.Expression);
                if (invokedMethodCall != null)
                {
                    return invokedMethodCall;
                }

                foreach (Expression argument in invocationExpression.Arguments)
                {
                    MethodCallExpression argumentMethodCall = FindMethodCall(argument);
                    if (argumentMethodCall != null)
                    {
                        return argumentMethodCall;
                    }
                }

                return null;
            }

            if (expression is MemberExpression memberExpression)
            {
                return FindMethodCall(memberExpression.Expression);
            }

            if (expression is BinaryExpression binaryExpression)
            {
                MethodCallExpression left = FindMethodCall(binaryExpression.Left);
                return left ?? FindMethodCall(binaryExpression.Right);
            }

            if (expression is ConditionalExpression conditionalExpression)
            {
                MethodCallExpression test = FindMethodCall(conditionalExpression.Test);
                if (test != null)
                {
                    return test;
                }

                MethodCallExpression ifTrue = FindMethodCall(conditionalExpression.IfTrue);
                return ifTrue ?? FindMethodCall(conditionalExpression.IfFalse);
            }

            if (expression is LambdaExpression lambdaExpression)
            {
                return FindMethodCall(lambdaExpression.Body);
            }

            return null;
        }

        /// <summary>
        /// Builds a list of cache arguments from the provided method call.
        /// </summary>
        /// <param name="methodCall">The method call expression.</param>
        /// <param name="cancellationTokenParameter">The cancellation token parameter to ignore.</param>
        /// <returns>The extracted argument list or null when empty.</returns>
        protected static object BuildFilters(MethodCallExpression methodCall, ParameterExpression cancellationTokenParameter)
        {
            ParameterInfo[] parameters = methodCall.Method.GetParameters();
            List<CacheArgument> items = new List<CacheArgument>();

            for (int i = 0; i < methodCall.Arguments.Count; i++)
            {
                Expression argumentExpression = methodCall.Arguments[i];
                if (argumentExpression is ParameterExpression parameterExpression && parameterExpression == cancellationTokenParameter)
                {
                    continue;
                }

                string parameterName = parameters[i].Name ?? $"arg{i}";
                object value = EvaluateExpression(argumentExpression);
                items.Add(new CacheArgument
                {
                    Name = parameterName,
                    Value = value,
                });
            }

            return items.Count == 0 ? null : items;
        }

        /// <summary>
        /// Evaluates an expression and returns its runtime value.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated value.</returns>
        protected static object EvaluateExpression(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            if (expression is ConstantExpression constantExpression)
            {
                return constantExpression.Value;
            }

            Expression convertedExpression = Expression.Convert(expression, typeof(object));
            Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(convertedExpression);
            Func<object> getter = lambda.Compile();
            return getter();
        }

        /// <summary>
        /// Builds a cache key using the hashed invocation details, team scope, and tags.
        /// </summary>
        /// <param name="hash">The hash of class name, method name, and serialized filters.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="tags">The tag segments already prefixed for the cache key.</param>
        /// <returns>The composed cache key.</returns>
        protected static string BuildKey(string hash, int teamId = 0, List<string> tags = null)
        {
            string resolvedHash = string.IsNullOrWhiteSpace(hash) ? string.Empty : hash;

            tags = tags ?? new List<string>();

            string tagSegment = tags.Count == 0
                ? string.Empty
                : $"|{string.Join("|", tags)}";
            return $"{PrefixKey}|{resolvedHash}|{PrefixTeam}{teamId}{tagSegment}";
        }

        /// <summary>
        /// Computes a hash for the provided string value.
        /// </summary>
        /// <param name="rawValue">The raw string to hash.</param>
        /// <returns>The computed hash.</returns>
        protected static string ComputeHash(string rawValue)
        {
            if (rawValue == null)
            {
                return null;
            }

            byte[] serializedBytes = Encoding.UTF8.GetBytes(rawValue);
            byte[] hashBytes = SHA256.HashData(serializedBytes);
            return Convert.ToHexString(hashBytes);
        }

        /// <summary>
        /// Resolves the default expiration duration from configuration.
        /// </summary>
        /// <param name="biaNetSection">The BiaNet configuration section.</param>
        /// <returns>The resolved expiration.</returns>
        protected static TimeSpan ResolveExpiration(BiaNetSection biaNetSection)
        {
            int? expirationSeconds = biaNetSection?.CommonFeatures?.HybridCache?.ExpirationSeconds;

            if (!expirationSeconds.HasValue)
            {
                return TimeSpan.FromSeconds(300);
            }

            return expirationSeconds.Value > 0 ? TimeSpan.FromSeconds(expirationSeconds.Value) : TimeSpan.Zero;
        }

        /// <summary>
        /// Resolves the default local cache expiration duration from configuration.
        /// </summary>
        /// <param name="biaNetSection">The configuration source.</param>
        /// <returns>The resolved local cache expiration.</returns>
        protected static TimeSpan ResolveLocalCacheExpiration(BiaNetSection biaNetSection)
        {
            int? expirationSeconds = biaNetSection?.CommonFeatures?.HybridCache?.LocalCacheExpirationSeconds;

            if (!expirationSeconds.HasValue)
            {
                return TimeSpan.FromSeconds(5);
            }

            return expirationSeconds.Value > 0 ? TimeSpan.FromSeconds(expirationSeconds.Value) : TimeSpan.Zero;
        }

        /// <summary>
        /// Resolves whether the distributed cache is active from the configuration.
        /// If the configuration value is not set, returns true by default.
        /// </summary>
        /// <param name="biaNetSection">The configuration source.</param>
        /// <returns>True if the distributed cache is active; otherwise, false.</returns>
        protected static bool ResolveDistributedCacheActive(BiaNetSection biaNetSection)
        {
            bool? isActive = biaNetSection?.CommonFeatures?.DistributedCache?.IsActive;
            return isActive.GetValueOrDefault(true);
        }

        /// <summary>
        /// Resolves the cache key using the factory expression, tags, and team scope.
        /// </summary>
        /// <typeparam name="T">The cached value type.</typeparam>
        /// <param name="factoryExpression">The factory expression.</param>
        /// <param name="tags">The cache tags.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>The resolved cache key.</returns>
        protected virtual string ResolveCacheKey<T>(
            Expression<Func<CancellationToken, ValueTask<T>>> factoryExpression,
            List<string> tags,
            int teamId)
        {
            CacheInvocationInfo invocationInfo = GetInvocationInfo(factoryExpression);
            string className = invocationInfo.ClassName;
            string methodName = invocationInfo.MethodName;
            string filtersSerialized = invocationInfo.Filters == null
                ? string.Empty
                : JsonConvert.SerializeObject(invocationInfo.Filters);
            string hashInput = $"{className}|{methodName}|{filtersSerialized}";
            this.logger.LogDebug("Input before ComputeHash: {HashInput}", hashInput);
            string filtersHash = ComputeHash(hashInput);

            List<string> resolvedTags = tags.Select(tag => $"{PrefixTag}{tag}").ToList();
            return BuildKey(filtersHash, teamId, resolvedTags);
        }

        /// <summary>
        /// Resolves the cache state based on configured defaults and overrides.
        /// </summary>
        /// <param name="expiration">The override expiration for distributed cache.</param>
        /// <param name="localCacheExpiration">The override expiration for local cache.</param>
        /// <returns>The resolved cache state.</returns>
        protected virtual CacheState ResolveCacheState(TimeSpan? expiration, TimeSpan? localCacheExpiration)
        {
            TimeSpan? resolvedExpiration = expiration ?? this.expiration;
            TimeSpan? resolvedLocalCacheExpiration = localCacheExpiration ?? this.localCacheExpiration;

            bool isCacheActive = this.isDistributedCacheActive && resolvedExpiration != TimeSpan.Zero;
            bool isLocalCacheActive = resolvedLocalCacheExpiration != TimeSpan.Zero;

            HybridCacheEntryFlags flags = GetHybridCacheEntryFlags(isCacheActive, isLocalCacheActive);

            return new CacheState(resolvedExpiration, resolvedLocalCacheExpiration, isCacheActive, isLocalCacheActive, flags);
        }

        /// <summary>
        /// Asynchronously removes cache entries for the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The System.Threading.CancellationToken used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async Task RemoveDistCacheEntriesAsync(Expression<Func<DistCache, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            if (!this.isDistributedCacheActive)
            {
                return;
            }

            // We delete directly from the DistCache table because deletion by tag does not work properly on the database cache.
            IQueryable<DistCache> query = this.unitOfWork.RetrieveSet<DistCache>()
                .Where(x => x.Id.StartsWith(PrefixKey));

            if (filter != null)
            {
                query = query.Where(filter);
            }

            await query.ExecuteDeleteAsync(cancellationToken);
        }

        /// <summary>
        /// Holds metadata about the cache invocation.
        /// </summary>
        protected sealed class CacheInvocationInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CacheInvocationInfo"/> class.
            /// </summary>
            /// <param name="className">The declaring class name.</param>
            /// <param name="methodName">The method name.</param>
            /// <param name="filters">The captured filter arguments.</param>
            public CacheInvocationInfo(string className, string methodName, object filters)
            {
                this.ClassName = className;
                this.MethodName = methodName;
                this.Filters = filters;
            }

            /// <summary>
            /// Gets the declaring class name for the cached call.
            /// </summary>
            public string ClassName { get; }

            /// <summary>
            /// Gets the method name for the cached call.
            /// </summary>
            public string MethodName { get; }

            /// <summary>
            /// Gets the captured filter arguments used for key generation.
            /// </summary>
            public object Filters { get; }
        }

        /// <summary>
        /// Represents a named argument captured for cache key generation.
        /// </summary>
        protected sealed class CacheArgument
        {
            /// <summary>
            /// Gets or sets the argument name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the argument value.
            /// </summary>
            public object Value { get; set; }
        }

        /// <summary>
        /// Represents resolved cache settings for a cache entry.
        /// </summary>
        protected sealed class CacheState
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CacheState"/> class.
            /// </summary>
            /// <param name="expiration">The distributed cache expiration.</param>
            /// <param name="localCacheExpiration">The local cache expiration.</param>
            /// <param name="isCacheActive">Whether distributed cache is active.</param>
            /// <param name="isLocalCacheActive">Whether local cache is active.</param>
            /// <param name="flags">The resolved hybrid cache entry flags.</param>
            public CacheState(
                TimeSpan? expiration,
                TimeSpan? localCacheExpiration,
                bool isCacheActive,
                bool isLocalCacheActive,
                HybridCacheEntryFlags flags)
            {
                this.Expiration = expiration;
                this.LocalCacheExpiration = localCacheExpiration;
                this.IsCacheActive = isCacheActive;
                this.IsLocalCacheActive = isLocalCacheActive;
                this.Flags = flags;
            }

            /// <summary>
            /// Gets the distributed cache expiration.
            /// </summary>
            public TimeSpan? Expiration { get; }

            /// <summary>
            /// Gets the local cache expiration.
            /// </summary>
            public TimeSpan? LocalCacheExpiration { get; }

            /// <summary>
            /// Gets a value indicating whether distributed cache is active.
            /// </summary>
            public bool IsCacheActive { get; }

            /// <summary>
            /// Gets a value indicating whether local cache is active.
            /// </summary>
            public bool IsLocalCacheActive { get; }

            /// <summary>
            /// Gets the resolved hybrid cache entry flags.
            /// </summary>
            public HybridCacheEntryFlags Flags { get; }
        }
    }
}
