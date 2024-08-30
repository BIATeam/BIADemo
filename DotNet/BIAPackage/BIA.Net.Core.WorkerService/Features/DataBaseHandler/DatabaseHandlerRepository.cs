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

    /// <summary>
    /// Database handler to track change in database and react on it.
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
        /// The read change request.
        /// </summary>
        private readonly string readChangeRequest;

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
        /// The SQL connection used by the SQL borker handler.
        /// </summary>
        private SqlConnection brokerSqlConnection;

        /// <summary>
        /// Indicates weither the current instance is disposed or not.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseHandlerRepository{T}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connectionString">The connectionString.</param>
        /// <param name="onChangeEventHandlerRequest">sql request. The action is lanch if the result change.</param>
        /// <param name="readChangeRequest">sql request to run when a change is detected, and pass it to change action. If null is send null to change action.</param>
        /// <param name="databaseEngine">Database engine to request.</param>
        /// <param name="sqlFilterNotificationInfos">Filter the SQL event action type. If null send all action. To use with SQL broker only.</param>
        /// <param name="usePolling">Optional. Indicates if polling method should be used. False by default.</param>
        /// <param name="pollingInterval">Optional. Interval of polling. 5 seconds by default.</param>
        protected DatabaseHandlerRepository(
            IServiceProvider serviceProvider,
            string connectionString,
            string databaseEngine,
            string onChangeEventHandlerRequest,
            string readChangeRequest,
            List<SqlNotificationInfo> sqlFilterNotificationInfos = null,
            bool usePolling = false,
            TimeSpan? pollingInterval = null)
        {
            this.serviceProvider = serviceProvider;
            this.connectionString = connectionString;
            this.databaseEngine = databaseEngine;
            this.onChangeEventHandlerRequest = onChangeEventHandlerRequest;
            this.readChangeRequest = readChangeRequest;
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

            if (string.IsNullOrEmpty(readChangeRequest))
            {
                throw new ArgumentNullException(nameof(readChangeRequest));
            }
        }

        /// <summary>
        /// Format the expected action function.
        /// </summary>
        /// <param name="reader">The reader for ligne impacted.</param>
        public delegate void ChangeHandler(DbDataReader reader);

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
        /// The SQL read change request.
        /// </summary>
        protected string ReadChangeRequest => this.readChangeRequest;

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

        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        public virtual void Start()
        {
            this.logger.LogInformation($"{nameof(this.Start)}");
            this.logger.LogInformation($"{nameof(this.connectionString)} = {this.connectionString}");
            this.logger.LogInformation($"{nameof(this.databaseEngine)} = {this.databaseEngine}");
            this.logger.LogInformation($"{nameof(this.onChangeEventHandlerRequest)} = {this.OnChangeEventHandlerRequest}");
            this.logger.LogInformation($"{nameof(this.readChangeRequest)} = {this.ReadChangeRequest}");
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
            this.SqlBrokerHandle();
        }

        /// <summary>
        /// Stop the process of event handler.
        /// </summary>
        public virtual void Stop()
        {
            string message = $"{nameof(this.Stop)}";
            this.logger.LogInformation(message);

            if (this.IsDatabaseEngineSqlServer && !this.usePolling)
            {
                SqlDependency.Stop(this.connectionString);
            }

            this.Dispose(true);
        }

        /// <summary>
        /// Handle the changes using SQL broker.
        /// </summary>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        protected virtual void SqlBrokerHandle()
        {
            this.logger.LogInformation($"{nameof(this.SqlBrokerHandle)}");

            this.brokerSqlConnection = new SqlConnection(this.connectionString);
            using var command = new SqlCommand(this.OnChangeEventHandlerRequest, this.brokerSqlConnection);
            this.brokerSqlConnection.Open();

            var sqlDependency = new SqlDependency(command);
            this.logger.LogInformation($"{nameof(sqlDependency)}.OnChange += new OnChangeEventHandler(this.{nameof(this.OnSqlDependencyChange)})");
            sqlDependency.OnChange += this.OnSqlDependencyChange;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Called when SQL dependency change with SQL broker.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSqlDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.logger.LogInformation($"{nameof(this.OnSqlDependencyChange)}");

            if (!this.IsValidSqlNotificationEvent(e))
            {
                this.logger.LogInformation($"Invalid SQL notification event");
                return;
            }

            using var command = new SqlCommand(this.ReadChangeRequest, this.brokerSqlConnection);
            using var reader = command.ExecuteReader();
            this.logger.LogInformation($"reader.HasRows = {reader.HasRows}");
            if (reader.Read() && this.IsValidSqlNotificationEvent(e))
            {
                this.logger.LogInformation($"this.{nameof(this.OnChange)}(reader)");
                this.OnChange?.Invoke(reader);
            }

            this.brokerSqlConnection.Close();
            this.brokerSqlConnection.Dispose();

            this.SqlBrokerHandle();
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

            this.pollingDbConnection = this.GetPollingDataDbConnection();
            await this.pollingDbConnection.OpenAsync();

            var previousData = await this.FetchPollingDataAsync();
            while (!this.pollingCancellationToken.IsCancellationRequested)
            {
                try
                {
                    var newData = await this.FetchPollingDataAsync();
                    if (this.HasChangedPollingData(previousData, newData))
                    {
                        await this.OnChangesPollingDataAsync();
                    }

                    previousData = newData;
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

        /// <summary>
        /// Retrive data based on the <see cref="OnChangeEventHandlerRequest"/> for polling.
        /// </summary>
        /// <returns>A list of data rows wrapped into Dictionnary of string|object, string corresponding to column name, object the column's value.</returns>
        protected virtual async Task<List<Dictionary<string, object>>> FetchPollingDataAsync()
        {
            this.logger.LogInformation(nameof(this.FetchPollingDataAsync));

            var data = new List<Dictionary<string, object>>();

            using var command = this.GetPollingDataDbCommand(this.OnChangeEventHandlerRequest, this.pollingDbConnection);
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
        /// Task executed when changes are detected while polling.
        /// Execute the <see cref="readChangeRequest"/> and raise the <see cref="OnChange"/> event.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        protected virtual async Task OnChangesPollingDataAsync()
        {
            this.logger.LogInformation(nameof(this.OnChangesPollingDataAsync));

            using var command = this.GetPollingDataDbCommand(this.ReadChangeRequest, this.pollingDbConnection);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                this.OnChange?.Invoke(reader);
                return;
            }

            this.OnChange?.Invoke(null);
        }

        /// <summary>
        /// Check if the <paramref name="previousData"/> and <paramref name="newData"/> are equals when polling.
        /// </summary>
        /// <param name="previousData">Previous polling data to compare.</param>
        /// <param name="newData">New polling data to compare.</param>
        /// <returns>A <see cref="bool"/> that indicates if any changed has opered.</returns>
        protected virtual bool HasChangedPollingData(List<Dictionary<string, object>> previousData, List<Dictionary<string, object>> newData)
        {
            this.logger.LogInformation(nameof(this.HasChangedPollingData));

            if (previousData.Count != newData.Count)
            {
                return true;
            }

            for (int rowIndex = 0; rowIndex < previousData.Count; rowIndex++)
            {
                var previousRow = previousData[rowIndex];
                var newRow = newData[rowIndex];
                foreach (var columnName in previousRow.Select(x => x.Key))
                {
                    if (!newRow.TryGetValue(columnName, out object newValue) || !this.ArePollingDataValueEquals(previousRow[columnName], newValue))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the value of polling's data are equals.
        /// </summary>
        /// <param name="previousValue">Previous polling data value.</param>
        /// <param name="newValue">New polling data value.</param>
        /// <returns>A <see cref="bool"/> that indicates if the data are equals.</returns>
        protected virtual bool ArePollingDataValueEquals(object previousValue, object newValue)
        {
            if (previousValue is byte[] previousValueAsByteArray)
            {
                return previousValueAsByteArray.SequenceEqual((byte[])newValue);
            }

            return previousValue.Equals(newValue);
        }

        /// <summary>
        /// Provide the <see cref="DbConnection"/> based on current <see cref="databaseEngine"/> for polling data.
        /// </summary>
        /// <returns><see cref="DbConnection"/>.</returns>
        /// <exception cref="NotSupportedException">If current <see cref="databaseEngine"/> is not supported.</exception>
        protected virtual DbConnection GetPollingDataDbConnection()
        {
            return this.databaseEngine.ToLower() switch
            {
                "sqlserver" => new SqlConnection(this.connectionString),
                "postgresql" => new Npgsql.NpgsqlConnection(this.connectionString),
                _ => throw new NotSupportedException()
            };
        }

        /// <summary>
        /// Provide the <see cref="DbCommand"/> based on current <see cref="databaseEngine"/> for polling data.
        /// </summary>
        /// <param name="command">The command to create.</param>
        /// <param name="connection">The <see cref="DbConnection"/> to use with the command.</param>
        /// <returns><see cref="DbCommand"/>.</returns>
        /// <exception cref="NotSupportedException">If current <see cref="databaseEngine"/> is not supported.</exception>
        protected virtual DbCommand GetPollingDataDbCommand(string command, DbConnection connection)
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