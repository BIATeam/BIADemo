using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    public class DatabaseHandlerOptions
    {
        // handler options
        internal bool IsActive { get; private set; }
        internal List<DatabaseHandlerRepository> DatabaseHandlerRepositories { get; private set; }

        public DatabaseHandlerOptions()
        {
            IsActive = false;
        }

        public void Activate(List<DatabaseHandlerRepository> databasehandlerRepositories)
        {
            IsActive = true;
            DatabaseHandlerRepositories = databasehandlerRepositories;
        }

    }
}
