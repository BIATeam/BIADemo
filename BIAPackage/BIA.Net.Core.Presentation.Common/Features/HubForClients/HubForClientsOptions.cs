using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Presentation.Common.Features.HubForClients
{
    public class HubForClientsOptions
    {
        public bool IsActive { get; private set; }
        public string RedisConnectionString { get; private set; }
        public string RedisChannelPrefix { get; private set; }

        public HubForClientsOptions()
        {
            IsActive = false;
        }

        public void Activate(string redisConnectionString, string redisChannelPrefix)
        {
            IsActive = true;
            RedisConnectionString = redisConnectionString;
            RedisChannelPrefix = redisChannelPrefix;
        }
    }
}
