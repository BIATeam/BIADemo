// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Database handler to track change in database and react on it.
    /// </summary>
    public class DatabaseHandlerRepository : IDatabaseHandlerRepository
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
        /// The logger.
        /// </summary>
        private ILogger<DatabaseHandlerRepository> logger;

        /// <summary>
        /// Detect if it is the first detection. (to ignore).
        /// </summary>
        private bool isFirst = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseHandlerRepository" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connectionString">The connectionString.</param>
        /// <param name="sqlOnChangeEventHandlerRequest">sql request. The action is lanch if the result change.</param>
        /// <param name="sqlReadChangeRequest">sql request to run when a change is detected, and pass it to change action. If null is send null to change action.</param>
        /// <param name="onChange">Action to perform on Change detected. The function take a reader parameter. Exemple of usage for a int : int id = reader.GetInt32(0).</param>
        /// <param name="filterNotifictionInfos">Filter the event action type. If null send all action.</param>
        public DatabaseHandlerRepository(
            string connectionString,
            SqlCommand sqlOnChangeEventHandlerRequest,
            SqlCommand sqlReadChangeRequest,
            ChangeHandler onChange,
            List<SqlNotificationInfo> filterNotifictionInfos = null)
        {
            this.connectionString = connectionString;
            this.sqlOnChangeEventHandlerRequest = sqlOnChangeEventHandlerRequest;
            this.sqlReadChangeRequest = sqlReadChangeRequest;
            this.OnChange = onChange;
            this.filterNotifictionInfos = filterNotifictionInfos;
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
            this.logger = serviceProvider.GetService<ILogger<DatabaseHandlerRepository>>();

            string message = $"{nameof(DatabaseHandlerRepository)}.{nameof(this.Start)}";
            this.logger.LogInformation(message);
            message = $"{nameof(this.connectionString)} = {this.connectionString}";
            this.logger.LogInformation(message);
            message = $"{nameof(this.sqlOnChangeEventHandlerRequest)} = {this.sqlOnChangeEventHandlerRequest.CommandText}";
            this.logger.LogInformation(message);
            message = $"{nameof(this.sqlReadChangeRequest)} = {this.sqlReadChangeRequest}";
            this.logger.LogInformation(message);

            this.NotifyNewItem(null);
        }

        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        public virtual void Stop()
        {
            string message = $"{nameof(DatabaseHandlerRepository)}.{nameof(this.Stop)}";
            this.logger.LogInformation(message);
            SqlDependency.Stop(this.connectionString);
        }

        /// <summary>
        /// Notifies the new item.
        /// </summary>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/> instance containing the event data.</param>
        protected virtual void NotifyNewItem(SqlNotificationEventArgs e)
        {
            string baseLog = $"{nameof(DatabaseHandlerRepository)}.{nameof(this.NotifyNewItem)}";

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
            this.logger.LogInformation($"{nameof(DatabaseHandlerRepository)}.{nameof(this.OnDependencyChange)}");

            this.isFirst = false;

            if (e.Info != SqlNotificationInfo.Invalid)
            {
                string message = $"{nameof(DatabaseHandlerRepository)}.{nameof(this.OnDependencyChange)} this.NotifyNewItem({e.Info});";
                this.logger.LogInformation(message);
                this.NotifyNewItem(e);
            }
        }
    }
}
