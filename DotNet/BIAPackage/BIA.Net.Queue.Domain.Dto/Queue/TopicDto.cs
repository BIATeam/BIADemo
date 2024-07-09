// <copyright file="TopicDto.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.Dto.Queue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Dto to define address of RabbitMQ server and topic.
    /// </summary>
    public class TopicDto
    {
        /// <summary>
        /// The RabbitMQ server URI.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The Virtual Host.
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// The exchange to listen.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The rabbitMQ routing key to listen.
        /// </summary>
        public string RoutingKey { get; set; }
    }
}