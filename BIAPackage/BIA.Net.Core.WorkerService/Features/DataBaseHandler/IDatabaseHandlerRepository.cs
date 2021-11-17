// <copyright file="DatabaseHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class DatabaseHandlerRepository 
    {
        private readonly string connectionString;

        private readonly string sqlOnChangeEventHandlerRequest;
        private readonly string sqlReadChangeRequest;
        private readonly List<SqlNotificationInfo> filterNotifictionInfos;
        public delegate void ChangeHandler(SqlDataReader reader);



        public event ChangeHandler OnChange;

        public DatabaseHandlerRepository(
            string connectionString, 
            string sqlOnChangeEventHandlerRequest, 
            string sqlReadChangeRequest, 
            ChangeHandler OnChange,
            List<SqlNotificationInfo> filterNotifictionInfos = null
            )
        {
            this.connectionString = connectionString;
            this.sqlOnChangeEventHandlerRequest = sqlOnChangeEventHandlerRequest;
            this.sqlReadChangeRequest = sqlReadChangeRequest;
            this.OnChange = OnChange;
            this.filterNotifictionInfos = filterNotifictionInfos;
        }

        private bool isFirst = true;

        public void Start()
        {
            this.NotifyNewItem(null);
        }

        private void NotifyNewItem(SqlNotificationEventArgs e)
        {
            if (this.isFirst)
            {
                SqlDependency.Start(connectionString);
            }

            using (SqlConnection connection = new(connectionString))
            using (SqlCommand command = new(sqlOnChangeEventHandlerRequest, connection))
            {
                connection.Open();

                SqlDependency dependency = new(command);
                dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange);
                command.ExecuteNonQuery();

                if (!this.isFirst)
                {
                    if (string.IsNullOrEmpty(sqlReadChangeRequest))
                    {
                        if (IsValidEvent(e) && this.OnChange != null)
                        {
                            this.OnChange(null);
                        }
                    }
                    else
                    {
                        using (SqlCommand selectCommand = new(sqlReadChangeRequest, connection))
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                //int id = reader.GetInt32(0);

                                if (IsValidEvent(e) && this.OnChange != null)
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
                        this.filterNotifictionInfos.Contains(e.Info)
                    );
        }

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.isFirst = false;

            if (e.Info != SqlNotificationInfo.Invalid)

            {
                this.NotifyNewItem(e);
            }
        }

        public void Stop()
        {
            SqlDependency.Stop(this.connectionString);
        }
    }
}
