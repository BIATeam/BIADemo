// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System.Collections.Generic;
    using System.Data.SqlClient;

    /// <summary>
    /// Database handler to track change in database and react on it.
    /// </summary>
    public class DatabaseHandlerRepository
    {
        private readonly string connectionString;

        private readonly string sqlOnChangeEventHandlerRequest;
        private readonly string sqlReadChangeRequest;
        private readonly List<SqlNotificationInfo> filterNotifictionInfos;

        /// <summary>
        /// Detect if it is the first detection. (to ignore).
        /// </summary>
        private bool isFirst = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseHandlerRepository"/> class.
        /// </summary>
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
        public event ChangeHandler OnChange;

        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        public void Start()
        {
            this.NotifyNewItem(null);
        }

        /// <summary>
        /// Start the process of event handler.
        /// </summary>
        public void Stop()
        {
            SqlDependency.Stop(this.connectionString);
        }

        private void NotifyNewItem(SqlNotificationEventArgs e)
        {
            if (this.isFirst)
            {
                SqlDependency.Start(this.connectionString);
            }

            using (SqlConnection connection = new (this.connectionString))
            using (SqlCommand command = new (this.sqlOnChangeEventHandlerRequest, connection))
            {
                connection.Open();

                SqlDependency dependency = new (command);
                dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange);
                command.ExecuteNonQuery();

                if (!this.isFirst)
                {
                    if (string.IsNullOrEmpty(this.sqlReadChangeRequest))
                    {
                        if (this.IsValidEvent(e) && this.OnChange != null)
                        {
                            this.OnChange(null);
                        }
                    }
                    else
                    {
                        using (SqlCommand selectCommand = new(this.sqlReadChangeRequest, connection))
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                if (this.IsValidEvent(e) && this.OnChange != null)
                                {
                                    this.OnChange(reader);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsValidEvent(SqlNotificationEventArgs e)
        {
            return (e != null)
                    &&
                    (
                        this.filterNotifictionInfos == null
                        ||
                        this.filterNotifictionInfos.Contains(e.Info));
        }

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.isFirst = false;

            if (e.Info != SqlNotificationInfo.Invalid)
            {
                this.NotifyNewItem(e);
            }
        }
    }
}
