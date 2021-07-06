// <copyright file="PlaneHandlerRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System.Data.SqlClient;

    public class DatabaseHandlerRepository 
    {
        private readonly string connectionString;

        private readonly string sqlOnChangeEventHandlerRequest;
        private readonly string sqlReadChangeRequest;

        public delegate void ChangeHandler(SqlDataReader reader);

        public event ChangeHandler OnChange;

        public DatabaseHandlerRepository(string connectionString, string sqlOnChangeEventHandlerRequest, string sqlReadChangeRequest, ChangeHandler OnChange)
        {
            this.connectionString = connectionString;
            this.sqlOnChangeEventHandlerRequest = sqlOnChangeEventHandlerRequest;
            this.sqlReadChangeRequest = sqlReadChangeRequest;
            this.OnChange = OnChange;
        }

        private bool isFirst = true;

        public void Start()
        {
            this.NotifyNewItem();
        }

        private void NotifyNewItem()
        {
            if (this.isFirst)
            {
                SqlDependency.Start(connectionString);
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sqlOnChangeEventHandlerRequest, connection))
            {
                connection.Open();

                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(this.OnDependencyChange);
                command.ExecuteNonQuery();

                if (!this.isFirst)
                {
                    if (string.IsNullOrEmpty(sqlReadChangeRequest))
                    {
                        if (this.OnChange != null)
                        {
                            this.OnChange(null);
                        }
                    }
                    else
                    {
                        using (SqlCommand selectCommand = new SqlCommand(sqlReadChangeRequest, connection))
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                //int id = reader.GetInt32(0);

                                if (this.OnChange != null)
                                {
                                    this.OnChange(reader);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            this.isFirst = false;

            if (e.Info != SqlNotificationInfo.Invalid)
            {
                this.NotifyNewItem();
            }
        }

        public void Stop()
        {
            SqlDependency.Stop(this.connectionString);
        }
    }
}
