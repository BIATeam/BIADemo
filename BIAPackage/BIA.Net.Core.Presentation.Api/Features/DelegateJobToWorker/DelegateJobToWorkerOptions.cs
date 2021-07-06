using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Presentation.Api.Features.DelegateJobToWorker
{
    public class DelegateJobToWorkerOptions
    {
        internal bool IsActive { get; private set; }
        internal string ConnectionStringName { get; private set; }

        public DelegateJobToWorkerOptions()
        {
            IsActive = false;
        }

        public void Activate(string connectionStringName)
        {
            IsActive = true;
            ConnectionStringName = connectionStringName;
        }
    }
}
