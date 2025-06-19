// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Database handler to track change in database and react on it.
    /// Default mode is using polling handler.
    /// </summary>
    /// <typeparam name="T">Type of inherited child.</typeparam>
#pragma warning disable CA2100
    public abstract class DatabaseHandlerRepository<T> : IDatabaseHandlerRepository, IDisposable
    {
        /// <summary>
        /// Identifier of database engine SQL Server.
        /// </summary>
        protected const string DbEngineSqlServer = "sqlserver";

        /// <summary>
        /// Identifier of database engine PotgreSQL.
        /// </summary>
        protected const string DbEnginePostgreSql = "postgresql";

        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The database engine type.
        /// </summary>
        private readonly string databaseEngine;

        /// <summary>
        /// The on change event handler request.
        /// </summary>
        private readonly string onChangeEventHandlerRequest;

        /// <summary>
        /// The index key of the on change event handler request.
        /// </summary>
        private readonly string indexKey;

        /// <summary>
        /// The SQL filter notification infos.
        /// </summary>
        private readonly List<SqlNotificationInfo> sqlFilterNotificationInfos;

        /// <summary>
        /// Indicates if SQL data broker mode should be used.
        /// </summary>
        private readonly bool useSqlDataBroker;

        /// <summary>
        /// Interval of polling.
        /// </summary>
        private readonly TimeSpan pollingInterval;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<T> logger;

        /// <summary>
        /// The polling cancellation token to stop the polling task.
        /// </summary>
        private CancellationTokenSource pollingCancellationToken;

        /// <summary>
        /// The Database Connection use by the polling handler.
        /// </summary>
        private DbConnection pollingDbConnection;

        /// <summary>
        /// The SQL connection used by the SQL broker handler.
        /// </summary>
        private SqlConnection brokerSqlConnection;

        /// <summary>
        /// The previous fetched data used by tehe SQL broker handler.
        /// </summary>
        private List<Dictionary<string, object>> brokerPreviousData;

        /// <summary>
        /// Indicates weither the current instance is disposed or not.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseHandlerRepository{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connectionString">The connectionString.</param>
        /// <param name="onChangeEventHandlerRequest">Database request to handle changes.</param>
        /// <param name="indexKey">Index key that must be included in the <paramref name="onChangeEventHandlerRequest"/>.</param>
        /// <param name="databaseEngine">Database engine to request.</param>
        /// <param name="useSqlDataBroker">Optional. Indicates if SQL data broker mode should be used instead of polling.</param>
        /// <param name="sqlFilterNotificationInfos">Optional. Filter the SQL event action type. If null send all action. To use with SQL broker only.</param>
        /// <param name="pollingInterval">Optional. Interval of polling. 5 seconds by default.</param>
        protected DatabaseHandlerRepository(
            IServiceProvider serviceProvider,
            string connectionString,
            string databaseEngine,
            string onChangeEventHandlerRequest,
            string indexKey,
            TimeSpan? pollingInterval = null,
            bool? useSqlDataBroker = false,
            List<SqlNotificationInfo> sqlFilterNotificationInfos = null)
        {
            this.serviceProvider = serviceProvider;
            this.connectionString = connectionString;
            this.databaseEngine = databaseEngine;
            this.useSqlDataBroker = useSqlDataBroker.GetValueOrDefault();
            this.onChangeEventHandlerRequest = onChangeEventHandlerRequest;
            this.indexKey = indexKey;

            this.sqlFilterNotificationInfos = sqlFilterNotificationInfos;
            this.pollingInterval = pollingInterval ?? TimeSpan.FromSeconds(5);
            this.logger = this.serviceProvider.GetService<ILogger<T>>();

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(databaseEngine))
            {
                throw new ArgumentNullException(nameof(databaseEngine));
            }

            if (string.IsNullOrEmpty(onChangeEventHandlerRequest))
            {
                throw new ArgumentNullException(nameof(onChangeEventHandlerRequest));
            }

            if (string.IsNullOrEmpty(indexKey))
            {
                throw new ArgumentNullException(nameof(onChangeEventHandlerRequest));
            }
        }

        /// <summary>
        /// The service provider.
        /// </summary>
        protected IServiceProvider ServiceProvider => this.serviceProvider;

        /// <summary>
        /// The connection string.
        /// </summary>
        protected string ConnectionString => this.connectionString;

        /// <summary>
        /// The database engine type.
        /// </summary>
        protected string DatabaseEngine => this.databaseEngine;

        /// <summary>
        /// The SQL on change event handler request.
        /// </summary>
        protected string OnChangeEventHandlerRequest => this.onChangeEventHandlerRequest;

        /// <summary>
        /// The SQL filter notification infos.
        /// </summary>
        protected List<SqlNotificationInfo> SqlFilterNotificationInfos => this.sqlFilterNotificationInfos;

        /// <summary>
        /// Indicates if data broker method should be used.
        /// </summary>
        protected bool UseDataBroker => this.useSqlDataBroker;

        /// <summary>
        /// Interval of polling.
        /// </summary>
        protected TimeSpan PollingInterval => this.pollingInterval;

        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger<T> Logger => this.logger;

        /// <summary>
        /// The polling cancellation token to stop the polling task.
        /// </summary>
        protected virtual CancellationTokenSource PollingCancellationToken => this.pollingCancellationToken;

        /// <summary>
        /// The Database Connection use by the polling handler.
        /// </summary>
        protected virtual DbConnection PollingDbConnection => this.pollingDbConnection;

        /// <summary>
        /// The SQL connection used by the SQL borker handler.
        /// </summary>
        protected virtual SqlConnection BrokerSqlConnection => this.brokerSqlConnection;

        /// <summary>
        /// Indicates if the database engine is SQL Server.
        /// </summary>
        protected virtual bool IsDatabaseEngineSqlServer => this.databaseEngine.Equals(DbEngineSqlServer, StringComparison.CurrentCultureIgnoreCase);

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public virtual async Task Start()
        {
            this.logger.LogInformation($"{nameof(this.Start)}");

            if (this.IsDatabaseEngineSqlServer && this.useSqlDataBroker)
            {
                this.logger.LogInformation("Using SQL Data Broker mode");
                SqlDependency.Start(this.connectionString);
                await this.SqlBrokerHandleAsync();
                return;
            }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.logger.LogInformation("Using polling mode");
            this.PollingHandleAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        /// <inheritdoc/>
        public virtual Task Stop()
        {
            string message = $"{nameof(this.Stop)}";
            this.logger.LogInformation(message);

            if (this.IsDatabaseEngineSqlServer && this.useSqlDataBroker)
            {
                SqlDependency.Stop(this.connectionString);
            }

            this.Dispose(true);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when the handler detect changes using the <see cref="OnChangeEventHandlerRequest"/>.
        /// </summary>
        /// <param name="changedData">The content of the <see cref="DataBaseHandlerChangedData"/>.</param>
        /// <returns>Completed <see cref="Task"/>.</returns>
        protected abstract Task OnChange(DataBaseHandlerChangedData changedData);

        /// <summary>
        /// Handle the changes using SQL broker.
        /// </summary>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        protected virtual async Task SqlBrokerHandleAsync()
        {
            this.brokerSqlConnection = new SqlConnection(this.connectionString);
            await this.brokerSqlConnection.OpenAsync();

            this.brokerPreviousData = await this.FetchDataAsync(this.brokerSqlConnection);

            using var command = new SqlCommand(this.OnChangeEventHandlerRequest, this.brokerSqlConnection);
            var sqlDependency = new SqlDependency(command);
            sqlDependency.OnChange += async (s, e) => await this.OnSqlDependencyChange(s, e);
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Called when SQL dependency change with SQL broker.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        protected virtual async Task OnSqlDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            if (!this.IsValidSqlNotificationEvent(e))
            {
                this.logger.LogInformation("SQL Notification Event invalid : Source={Source}, Type={Type}, Info={Info}", e.Source, e.Type, e.Info);
                return;
            }

            var currentData = await this.FetchDataAsync(this.brokerSqlConnection);
            this.HandleChangedData(this.brokerPreviousData, currentData);
            this.brokerPreviousData = currentData;

            await this.brokerSqlConnection.CloseAsync();
            await this.brokerSqlConnection.DisposeAsync();

            await this.SqlBrokerHandleAsync();
        }

        /// <summary>
        /// Determines whether is valid SQL notification event raised when SQL dependency changed with SQL broker.
        /// </summary>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        /// <returns>
        ///   <c>true</c> if [is valid event] [the specified e]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsValidSqlNotificationEvent(SqlNotificationEventArgs e)
        {
            var isValidEvent =
                e != null
                && e.Info != SqlNotificationInfo.Invalid
                && (this.sqlFilterNotificationInfos == null || this.sqlFilterNotificationInfos.Contains(e.Info));

            return isValidEvent;
        }

        /// <summary>
        /// Handle changes using polling method.
        /// </summary>
        /// <returns><see cref="Task"/> with polling method.</returns>
        protected virtual async Task PollingHandleAsync()
        {
            this.pollingCancellationToken = new CancellationTokenSource();
            this.pollingDbConnection = this.GetDbConnection();

            await this.pollingDbConnection.OpenAsync();
            var previousData = await this.FetchDataAsync(this.pollingDbConnection);

            while (!this.pollingCancellationToken.IsCancellationRequested)
            {
                try
                {
                    var currentData = await this.FetchDataAsync(this.pollingDbConnection);
                    this.HandleChangedData(previousData, currentData);
                    previousData = currentData;
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Polling failed : {Ex}", ex);
                }
                finally
                {
                    await Task.Delay(this.pollingInterval);
                }
            }
        }

        /// <summary>
        /// Retrieve data based on the <see cref="OnChangeEventHandlerRequest"/>.
        /// </summary>
        /// <param name="dbConnection">The <see cref="DbConnection"/> to use to execute the fetch command.</param>
        /// <returns>A list of data rows wrapped into <see cref="Dictionary{TKey, TValue}"/>, string corresponding to column name, object the column's value.</returns>
        protected virtual async Task<List<Dictionary<string, object>>> FetchDataAsync(DbConnection dbConnection)
        {
            var data = new List<Dictionary<string, object>>();

            using var command = this.GetDbCommand(this.OnChangeEventHandlerRequest, dbConnection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var rowData = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    rowData.Add(reader.GetName(i), reader.GetValue(i));
                }

                data.Add(rowData);
            }

            return data;
        }

        /// <summary>
        /// Handle any changed between the <paramref name="previousDataSet"/> and the <paramref name="currentDataSet"/>.
        /// </summary>
        /// <param name="previousDataSet">Set of previous data to compare.</param>
        /// <param name="currentDataSet">Set of current data to compare.</param>
        protected virtual void HandleChangedData(List<Dictionary<string, object>> previousDataSet, List<Dictionary<string, object>> currentDataSet)
        {
            this.GetChangedData(previousDataSet, currentDataSet).ForEach(x =>
            {
                this.logger.LogInformation("Changed data: {SerializeObject}", JsonConvert.SerializeObject(x));
                this.OnChange(x);
            });
        }

        /// <summary>
        /// Produces the list of changed data based on the <paramref name="previousDataSet"/> and <paramref name="currentDataSet"/>.
        /// </summary>
        /// <param name="previousDataSet">Previous data to compare.</param>
        /// <param name="currentDataSet">Current data to compare.</param>
        /// <returns>The list of <see cref="DataBaseHandlerChangedData"/>.</returns>
        protected virtual List<DataBaseHandlerChangedData> GetChangedData(List<Dictionary<string, object>> previousDataSet, List<Dictionary<string, object>> currentDataSet)
        {
            var changedData = new List<DataBaseHandlerChangedData>();
            var previousDataDictionary = previousDataSet.ToDictionary(data => data[this.indexKey].ToString());
            var currentDataDictionary = currentDataSet.ToDictionary(data => data[this.indexKey].ToString());

            foreach (var previousDataIndexKey in previousDataDictionary.Keys)
            {
                var previousData = previousDataDictionary[previousDataIndexKey];
                if (!currentDataDictionary.TryGetValue(previousDataIndexKey, out Dictionary<string, object> currentData))
                {
                    changedData.Add(new DataBaseHandlerChangedData(DatabaseHandlerChangeType.Delete, previousData: previousData));
                    continue;
                }

                foreach (var key in previousData.Keys)
                {
                    var previousDataValue = previousData[key];
                    if (!currentData.TryGetValue(key, out object currentDataValue) || !this.AreDataValueEquals(previousDataValue, currentDataValue))
                    {
                        changedData.Add(new DataBaseHandlerChangedData(DatabaseHandlerChangeType.Modify, previousData, currentData));
                        break;
                    }
                }
            }

            changedData.AddRange(from currentDataIndexKey in currentDataDictionary.Keys
                                 where !previousDataDictionary.ContainsKey(currentDataIndexKey)
                                 select new DataBaseHandlerChangedData(DatabaseHandlerChangeType.Add, currentData: currentDataDictionary[currentDataIndexKey]));
            return changedData;
        }

        /// <summary>
        /// Check if the value of data are equals.
        /// </summary>
        /// <param name="previousValue">Previous data value.</param>
        /// <param name="currentValue">Current data value.</param>
        /// <returns>A <see cref="bool"/> that indicates if the data are equals.</returns>
        protected virtual bool AreDataValueEquals(object previousValue, object currentValue)
        {
            if (previousValue == null || currentValue == null)
            {
                return previousValue == currentValue;
            }

            if (previousValue is byte[] previousValueAsByteArray && currentValue is byte[] currentValueAsByteArray)
            {
                return previousValueAsByteArray.SequenceEqual(currentValueAsByteArray);
            }

            if (previousValue is float previousValueAsFloat && currentValue is float currentValueAsFloat)
            {
                return Math.Abs(previousValueAsFloat - currentValueAsFloat) < float.Epsilon;
            }

            if (previousValue is double previousValueAsDouble && currentValue is double currentValueAsDouble)
            {
                return Math.Abs(previousValueAsDouble - currentValueAsDouble) < double.Epsilon;
            }

            if (previousValue is string previousValueAsString && currentValue is string currentValueAsString)
            {
                return string.Equals(previousValueAsString, currentValueAsString, StringComparison.OrdinalIgnoreCase);
            }

            return previousValue.Equals(currentValue);
        }

        /// <summary>
        /// Provide the <see cref="DbConnection"/> based on current <see cref="databaseEngine"/>.
        /// </summary>
        /// <returns><see cref="DbConnection"/>.</returns>
        /// <exception cref="NotSupportedException">If current <see cref="databaseEngine"/> is not supported.</exception>
        protected virtual DbConnection GetDbConnection()
        {
            return this.databaseEngine.ToLower() switch
            {
                DbEngineSqlServer => new SqlConnection(this.connectionString),
                DbEnginePostgreSql => new Npgsql.NpgsqlConnection(this.connectionString),
                _ => throw new NotSupportedException()
            };
        }

        /// <summary>
        /// Provide the <see cref="DbCommand"/> based on current <see cref="databaseEngine"/>.
        /// </summary>
        /// <param name="command">The command to create.</param>
        /// <param name="connection">The <see cref="DbConnection"/> to use with the command.</param>
        /// <returns><see cref="DbCommand"/>.</returns>
        /// <exception cref="NotSupportedException">If current <see cref="databaseEngine"/> is not supported.</exception>
        protected virtual DbCommand GetDbCommand(string command, DbConnection connection)
        {
            return this.databaseEngine.ToLower() switch
            {
                DbEngineSqlServer => new SqlCommand(command, connection as SqlConnection),
                DbEnginePostgreSql => new Npgsql.NpgsqlCommand(command, connection as Npgsql.NpgsqlConnection),
                _ => throw new NotSupportedException()
            };
        }

        /// <summary>
        /// Dispose or not the current instance managed resources.
        /// </summary>
        /// <param name="disposing"><see cref="bool"/> that will dispose or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.brokerSqlConnection != null)
                {
                    this.brokerSqlConnection.Close();
                    this.brokerSqlConnection.Dispose();
                }

                if (this.pollingDbConnection != null)
                {
                    this.pollingDbConnection.Close();
                    this.pollingDbConnection.Dispose();
                }

                if (this.pollingCancellationToken != null)
                {
                    this.pollingCancellationToken.Cancel();
                    this.pollingCancellationToken.Dispose();
                }
            }

            this.isDisposed = true;
        }
    }
}
#pragma warning restore CA2100