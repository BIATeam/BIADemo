using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Features.ClientForHub
{
    public class ClientForHubOptions
    {
        // hub for clients options
        public static bool IsActive { get; private set; }
        public static string SignalRUrl { get; private set; }

        public ClientForHubOptions()
        {
            IsActive = false;
        }

        public static void Activate(string signalRUrl)
        {
            IsActive = true;
            SignalRUrl = signalRUrl;
        }
    }
}
