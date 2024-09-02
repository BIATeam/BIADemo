// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
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
    /// Database handler to track change in database and react on it. Default method is using Sql broker handler.
    /// </summary>
    /// <typeparam name="T">Type of inherited child.</typeparam>
#pragma warning disable CA2100
    public abstract class DatabaseHandlerRepository<T> : IDatabaseHandlerRepository, IDisposable
    {
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
        /// Indicates if polling method should be used.
        /// </summary>
        private readonly bool usePolling;

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
        /// <param name="sqlFilterNotificationInfos">Optional. Filter the SQL event action type. If null send all action. To use with SQL broker only.</param>
        /// <param name="usePolling">Optional. Indicates if polling method should be used. False by default.</param>
        /// <param name="pollingInterval">Optional. Interval of polling. 5 seconds by default.</param>
        protected DatabaseHandlerRepository(
            IServiceProvider serviceProvider,
            string connectionString,
            string databaseEngine,
            string onChangeEventHandlerRequest,
            string indexKey,
            List<SqlNotificationInfo> sqlFilterNotificationInfos = null,
            bool usePolling = false,
            TimeSpan? pollingInterval = null)
        {
            this.serviceProvider = serviceProvider;
            this.connectionString = connectionString;
            this.databaseEngine = databaseEngine;
            this.onChangeEventHandlerRequest = onChangeEventHandlerRequest;
            this.indexKey = indexKey;

            this.sqlFilterNotificationInfos = sqlFilterNotificationInfos;
            this.usePolling = usePolling;
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
        /// Format the expected action function.
        /// </summary>
        /// <param name="changedData">The <see cref="DataBaseHandlerChangedData"/>.</param>
        public delegate void ChangeHandler(DataBaseHandlerChangedData changedData);

        /// <summary>
        /// The on change function.
        /// </summary>
        protected event ChangeHandler OnChange;

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
        /// Indicates if polling method should be used.
        /// </summary>
        protected bool UsePolling => this.usePolling;

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
        protected CancellationTokenSource PollingCancellationToken => this.pollingCancellationToken;

        /// <summary>
        /// The Database Connection use by the polling handler.
        /// </summary>
        protected DbConnection PollingDbConnection => this.pollingDbConnection;

        /// <summary>
        /// The SQL connection used by the SQL borker handler.
        /// </summary>
        protected SqlConnection BrokerSqlConnection => this.brokerSqlConnection;

        /// <summary>
        /// Indicates if the database engine is SQL Server.
        /// </summary>
        protected bool IsDatabaseEngineSqlServer => this.databaseEngine.Equals("sqlserver", StringComparison.CurrentCultureIgnoreCase);

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
            this.logger.LogInformation($"{nameof(this.connectionString)} = {this.connectionString}");
            this.logger.LogInformation($"{nameof(this.databaseEngine)} = {this.databaseEngine}");
            this.logger.LogInformation($"{nameof(this.onChangeEventHandlerRequest)} = {this.OnChangeEventHandlerRequest}");
            this.logger.LogInformation($"{nameof(this.usePolling)} = {this.usePolling}");

            if (!this.IsDatabaseEngineSqlServer || this.usePolling)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                this.PollingHandleAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return;
            }

            this.logger.LogInformation($"{nameof(SqlDependency)}.{nameof(SqlDependency.Start)}");
            SqlDependency.Start(this.connectionString);
            await this.SqlBrokerHandle();
        }

        /// <inheritdoc/>
        public virtual Task Stop()
        {
            string message = $"{nameof(this.Stop)}";
            this.logger.LogInformation(message);

            if (this.IsDatabaseEngineSqlServer && !this.usePolling)
            {
                SqlDependency.Stop(this.connectionString);
            }

            this.Dispose(true);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handle the changes using SQL broker.
        /// </summary>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        protected virtual async Task SqlBrokerHandle()
        {
            this.logger.LogInformation($"{nameof(this.SqlBrokerHandle)}");

            this.brokerSqlConnection = new SqlConnection(this.connectionString);
            await this.brokerSqlConnection.OpenAsync();

            using var command = new SqlCommand(this.OnChangeEventHandlerRequest, this.brokerSqlConnection);
            var sqlDependency = new SqlDependency(command);
            this.logger.LogInformation($"{nameof(sqlDependency)}.OnChange += new OnChangeEventHandler(this.{nameof(this.OnSqlDependencyChange)})");
            sqlDependency.OnChange += async (s, e) => await this.OnSqlDependencyChange(s, e);
            await command.ExecuteNonQueryAsync();

            this.brokerPreviousData = await this.FetchDataAsync(this.brokerSqlConnection);
        }

        /// <summary>
        /// Called when SQL dependency change with SQL broker.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        protected virtual async Task OnSqlDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.logger.LogInformation($"{nameof(this.OnSqlDependencyChange)}");

            if (!this.IsValidSqlNotificationEvent(e))
            {
                this.logger.LogInformation($"Invalid SQL notification event");
                return;
            }

            var currentData = await this.FetchDataAsync(this.brokerSqlConnection);
            var changedData = this.GetChangedData(this.brokerPreviousData, currentData);
            if (changedData.Count != 0)
            {
                this.OnChangedData(changedData);
            }

            this.brokerPreviousData = currentData;

            await this.brokerSqlConnection.CloseAsync();
            await this.brokerSqlConnection.DisposeAsync();

            await this.SqlBrokerHandle();
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

            this.logger.LogInformation($"{nameof(isValidEvent)} = {isValidEvent}");
            return isValidEvent;
        }

        /// <summary>
        /// Handle changes using polling method.
        /// </summary>
        /// <returns><see cref="Task"/> with polling method.</returns>
        protected virtual async Task PollingHandleAsync()
        {
            this.logger.LogInformation(nameof(this.PollingHandleAsync));

            this.pollingCancellationToken = new CancellationTokenSource();

            this.pollingDbConnection = this.GetDbConnection();

            try
            {
                await this.pollingDbConnection.OpenAsync();
                var previousData = await this.FetchDataAsync(this.pollingDbConnection);

                while (!this.pollingCancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var currentData = await this.FetchDataAsync(this.pollingDbConnection);
                        var changedData = this.GetChangedData(previousData, currentData);
                        if (changedData.Count != 0)
                        {
                            this.OnChangedData(changedData);
                        }

                        previousData = currentData;
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError($"Polling error : {ex}");
                    }
                    finally
                    {
                        await Task.Delay(this.pollingInterval);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Retrive data based on the <see cref="OnChangeEventHandlerRequest"/>.
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

            foreach (var currentDataIndexKey in currentDataDictionary.Keys)
            {
                if (!previousDataDictionary.ContainsKey(currentDataIndexKey))
                {
                    changedData.Add(new DataBaseHandlerChangedData(DatabaseHandlerChangeType.Add, currentData: currentDataDictionary[currentDataIndexKey]));
                }
            }

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
        /// Handle when there is changed data in the provider <paramref name="changedData"/> list.
        /// </summary>
        /// <param name="changedData">The list of <see cref="DataBaseHandlerChangedData"/>.</param>
        protected virtual void OnChangedData(List<DataBaseHandlerChangedData> changedData)
        {
            this.logger.LogInformation(nameof(this.OnChangedData));
            changedData.ForEach(x =>
            {
                this.logger.LogInformation($"Changed data: {JsonConvert.SerializeObject(x)}");
                this.OnChange?.Invoke(x);
            });
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
                "sqlserver" => new SqlConnection(this.connectionString),
                "postgresql" => new Npgsql.NpgsqlConnection(this.connectionString),
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
                "sqlserver" => new SqlCommand(command, connection as SqlConnection),
                "postgresql" => new Npgsql.NpgsqlCommand(command, connection as Npgsql.NpgsqlConnection),
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