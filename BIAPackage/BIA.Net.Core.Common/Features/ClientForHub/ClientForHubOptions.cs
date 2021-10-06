using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Features.ClientForHub
{
    public class ClientForHubOptions
    {
        // hub for clients options
        private static bool isActive = false;
        public static bool IsActive { get { return isActive; } private set { isActive = value; } }
        public static string SignalRUrl { get; private set; }
        public ClientForHubOptions()
        {
        }
        public static void Activate(string signalRUrl)
        {
            IsActive = true;
            SignalRUrl = signalRUrl;
        }
    }
}
