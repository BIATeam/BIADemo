using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.WorkerService.Features.ClientForHub
{
    public class ClientForHubOptions
    {
        // hub for clients options
        internal bool IsActive { get; private set; }
        internal string SignalRUrl { get; private set; }

        public ClientForHubOptions()
        {
            IsActive = false;
        }

        public void Activate(string signalRUrl)
        {
            IsActive = true;
            SignalRUrl = signalRUrl;
        }
    }
}
