// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Database handler to track change in database and react on it.
    /// </summary>
    public class DatabaseHandlerRepository
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        protected readonly string connectionString;

        /// <summary>
        /// The SQL on change event handler request.
        /// </summary>
        protected readonly string sqlOnChangeEventHandlerRequest;

        /// <summary>
        /// The SQL read change request.
        /// </summary>
        protected readonly string sqlReadChangeRequest;

        /// <summary>
        /// The filter notifiction infos.
        /// </summary>
        protected readonly List<SqlNotificationInfo> filterNotifictionInfos;

        /// <summary>
        /// The service provider.
        /// </summary>
        protected IServiceProvider serviceProvider;

        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger<DatabaseHandlerRepository> logger;

        /// <summary>
        /// Detect if it is the first detection. (to ignore).
        /// </summary>
        protected bool isFirst = true;

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
            string sqlOnChangeEventHandlerRequest,
            string sqlReadChangeRequest,
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
        /// Start the process of event handler.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public virtual void Start(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.logger = this.serviceProvider.GetService<ILogger<DatabaseHandlerRepository>>();

            this.logger.LogInformation($"{nameof(DatabaseHandlerRepository)}.{nameof(this.Start)}");
            this.logger.LogInformation($"{nameof(connectionString)} = {connectionString}");
            this.logger.LogInformation($"{nameof(sqlOnChangeEventHandlerRequest)} = {sqlOnChangeEventHandlerRequest}");
            this.logger.LogInformation($"{nameof(sqlReadChangeRequest)} = {sqlReadChangeRequest}");

            this.NotifyNewItem(null);
        }

        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        public virtual void Stop()
        {
            this.logger.LogInformation($"{nameof(DatabaseHandlerRepository)}.{nameof(this.Stop)}");
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
                this.logger.LogInformation($"{baseLog} {nameof(SqlDependency)}.{nameof(SqlDependency.Start)}");
                SqlDependency.Start(this.connectionString);
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand command = new SqlCommand(this.sqlOnChangeEventHandlerRequest, connection))
                {
                    connection.Open();

                    SqlDependency dependency = new SqlDependency(command);
                    this.logger.LogInformation($"{baseLog} dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange)");
                    dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange);
                    command.ExecuteNonQuery();

                    if (!this.isFirst)
                    {
                        if (string.IsNullOrEmpty(this.sqlReadChangeRequest))
                        {
                            if (this.IsValidEvent(e) && this.OnChange != null)
                            {
                                this.logger.LogInformation($"{baseLog} this.OnChange(null)");
                                this.OnChange(null);
                            }
                        }
                        else
                        {
                            using (SqlCommand selectCommand = new SqlCommand(this.sqlReadChangeRequest, connection))
                            {
                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                {
                                    this.logger.LogInformation($"{baseLog} reader.HasRows = {reader.HasRows}");
                                    if (reader.HasRows)
                                    {
                                        reader.Read();

                                        if (this.IsValidEvent(e) && this.OnChange != null)
                                        {
                                            this.logger.LogInformation($"{baseLog} this.OnChange(reader)");
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

            this.logger.LogInformation($"{nameof(isValidEvent)} = {isValidEvent}");

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
                this.logger.LogInformation($"{nameof(DatabaseHandlerRepository)}.{nameof(this.OnDependencyChange)} this.NotifyNewItem({e.Info});");
                this.NotifyNewItem(e);
            }
        }
    }
}
