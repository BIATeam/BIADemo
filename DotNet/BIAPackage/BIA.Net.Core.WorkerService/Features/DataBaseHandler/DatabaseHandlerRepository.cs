// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Database handler to track change in database and react on it.
    /// </summary>
    /// <typeparam name="T">Type of inherited child.</typeparam>
    public abstract class DatabaseHandlerRepository<T> : IDatabaseHandlerRepository
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The SQL on change event handler request.
        /// </summary>
        private readonly SqlCommand sqlOnChangeEventHandlerRequest;

        /// <summary>
        /// The SQL read change request.
        /// </summary>
        private readonly SqlCommand sqlReadChangeRequest;

        /// <summary>
        /// The filter notifiction infos.
        /// </summary>
        private readonly List<SqlNotificationInfo> filterNotifictionInfos;

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
        private ILogger<T> logger;

        /// <summary>
        /// Detect if it is the first detection. (to ignore).
        /// </summary>
        private bool isFirst = true;

        /// <summary>
        /// The polling cancellation token to stop the polling task.
        /// </summary>
        private CancellationTokenSource pollingCancellationToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseHandlerRepository{T}"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString.</param>
        /// <param name="sqlOnChangeEventHandlerRequest">sql request. The action is lanch if the result change.</param>
        /// <param name="sqlReadChangeRequest">sql request to run when a change is detected, and pass it to change action. If null is send null to change action.</param>
        /// <param name="onChange">Action to perform on Change detected. The function take a reader parameter. Exemple of usage for a int : int id = reader.GetInt32(0).</param>
        /// <param name="filterNotificationInfos">Filter the event action type. If null send all action.</param>
        /// <param name="usePolling">Optional. Indicates if polling method should be used. False by default.</param>
        /// <param name="pollingInterval">Optional. Interval of polling. 5 seconds by default.</param>
        protected DatabaseHandlerRepository(
            string connectionString,
            SqlCommand sqlOnChangeEventHandlerRequest,
            SqlCommand sqlReadChangeRequest,
            List<SqlNotificationInfo> filterNotificationInfos = null,
            bool usePolling = false,
            TimeSpan? pollingInterval = null)
        {
            this.connectionString = connectionString;
            this.sqlOnChangeEventHandlerRequest = sqlOnChangeEventHandlerRequest;
            this.sqlReadChangeRequest = sqlReadChangeRequest;
            this.filterNotifictionInfos = filterNotificationInfos;
            this.usePolling = usePolling;
            this.pollingInterval = pollingInterval ?? TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Format the expected action function.
        /// </summary>
        /// <param name="reader">The reader for ligne impacted.</param>
        public delegate void ChangeHandler(SqlDataReader reader);

        /// <summary>
        /// The on change function.
        /// </summary>
        protected event ChangeHandler OnChange;

        /// <summary>
        /// The connection string.
        /// </summary>
        protected string ConnectionString => this.connectionString;

        /// <summary>
        /// The SQL on change event handler request.
        /// </summary>
        protected SqlCommand SqlOnChangeEventHandlerRequest => this.sqlOnChangeEventHandlerRequest;

        /// <summary>
        /// The SQL read change request.
        /// </summary>
        protected SqlCommand SqlReadChangeRequest => this.sqlReadChangeRequest;

        /// <summary>
        /// The filter notifiction infos.
        /// </summary>
        protected List<SqlNotificationInfo> FilterNotifictionInfos => this.filterNotifictionInfos;

        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public virtual void Start(IServiceProvider serviceProvider)
        {
            this.logger = serviceProvider.GetService<ILogger<T>>();

            string message = $"{nameof(this.Start)}";
            this.logger.LogInformation(message);
            message = $"{nameof(this.connectionString)} = {this.connectionString}";
            this.logger.LogInformation(message);
            message = $"{nameof(this.sqlOnChangeEventHandlerRequest)} = {this.sqlOnChangeEventHandlerRequest.CommandText}";
            this.logger.LogInformation(message);
            message = $"{nameof(this.sqlReadChangeRequest)} = {this.sqlReadChangeRequest.CommandText}";
            this.logger.LogInformation(message);
            message = $"{nameof(this.usePolling)} = {this.usePolling}";
            this.logger.LogInformation(message);

            if (this.usePolling)
            {
                this.PollingHandleAsync();
                return;
            }

            this.BrokerHandle(null);
        }

        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        public virtual void Stop()
        {
            string message = $"{nameof(this.Stop)}";
            this.logger.LogInformation(message);

            if (this.usePolling)
            {
                this.pollingCancellationToken.Cancel();
                this.pollingCancellationToken.Dispose();
                return;
            }

            SqlDependency.Stop(this.connectionString);
        }

        /// <summary>
        /// Handle the changes using broker method.
        /// </summary>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        protected virtual void BrokerHandle(SqlNotificationEventArgs e)
        {
            string baseLog = $"{nameof(this.BrokerHandle)}";

            this.logger.LogInformation(baseLog);

            if (this.isFirst)
            {
                string message = $"{baseLog} {nameof(SqlDependency)}.{nameof(SqlDependency.Start)}";
                this.logger.LogInformation(message);
                SqlDependency.Start(this.connectionString);
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                this.sqlOnChangeEventHandlerRequest.Connection = connection;
                using (SqlCommand command = this.sqlOnChangeEventHandlerRequest.Clone())
                {
                    connection.Open();
                    command.Connection = connection;

                    SqlDependency dependency = new SqlDependency(command);
                    string message = $"{baseLog} dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange)";
                    this.logger.LogInformation(message);
                    dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange);
                    command.ExecuteNonQuery();

                    if (!this.isFirst)
                    {
                        if (string.IsNullOrEmpty(this.sqlReadChangeRequest.CommandText))
                        {
                            if (this.IsValidEvent(e) && this.OnChange != null)
                            {
                                string message1 = $"{baseLog} this.OnChange(null)";
                                this.logger.LogInformation(message1);
                                this.OnChange(null);
                            }
                        }
                        else
                        {
                            using (SqlCommand selectCommand = this.sqlReadChangeRequest.Clone())
                            {
                                selectCommand.Connection = connection;
                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                {
                                    string message1 = $"{baseLog} reader.HasRows = {reader.HasRows}";
                                    this.logger.LogInformation(message1);
                                    if (reader.HasRows)
                                    {
                                        reader.Read();

                                        if (this.IsValidEvent(e) && this.OnChange != null)
                                        {
                                            string message2 = $"{baseLog} this.OnChange(reader)";
                                            this.logger.LogInformation(message2);
                                            this.OnChange(reader);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is valid event] [the specified e].
        /// </summary>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        /// <returns>
        ///   <c>true</c> if [is valid event] [the specified e]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsValidEvent(SqlNotificationEventArgs e)
        {
            bool isValidEvent = (e != null)
                    &&
                    (
                        this.filterNotifictionInfos == null
                        ||
                        this.filterNotifictionInfos.Contains(e.Info));

            string message = $"{nameof(isValidEvent)} = {isValidEvent}";
            this.logger.LogInformation(message);

            return isValidEvent;
        }

        /// <summary>
        /// Called when [dependency change].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.logger.LogInformation($"{nameof(this.OnDependencyChange)}");
            this.isFirst = false;

            if (e.Info != SqlNotificationInfo.Invalid)
            {
                this.BrokerHandle(e);
            }
        }

        /// <summary>
        /// Handle changes using polling method.
        /// </summary>
        /// <returns><see cref="Task"/> with polling method.</returns>
        protected virtual async Task PollingHandleAsync()
        {
            this.pollingCancellationToken = new CancellationTokenSource();

            this.logger.LogInformation(nameof(this.PollingHandleAsync));
            var previousData = await this.FetchPollingDataAsync();

            while (!this.pollingCancellationToken.IsCancellationRequested)
            {
                try
                {
                    var newData = await this.FetchPollingDataAsync();
                    if (this.HasChangedPollingData(previousData, newData))
                    {
                        this.logger.LogInformation("Changed data");
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
        /// Retrive data based on the <see cref="SqlOnChangeEventHandlerRequest"/> for polling.
        /// </summary>
        /// <returns>A list of data rows wrapped into Dictionnary of string|object, string corresponding to column name, object the column's value.</returns>
        protected virtual async Task<List<Dictionary<string, object>>> FetchPollingDataAsync()
        {
            this.logger.LogInformation(nameof(this.FetchPollingDataAsync));

            var data = new List<Dictionary<string, object>>();

            using var connection = new SqlConnection(this.ConnectionString);
            using var command = this.sqlOnChangeEventHandlerRequest.Clone();
            await connection.OpenAsync();
            command.Connection = connection;
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
        /// Execute the <see cref="sqlReadChangeRequest"/> and raise the <see cref="OnChange"/> event.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        protected virtual async Task OnChangesPollingDataAsync()
        {
            this.logger.LogInformation(nameof(this.FetchPollingDataAsync));

            using var connection = new SqlConnection(this.ConnectionString);
            using var command = this.sqlReadChangeRequest.Clone();
            await connection.OpenAsync();
            command.Connection = connection;
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                this.OnChange(reader);
                return;
            }

            this.OnChange(null);
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
    }
}
