// <copyright file="QueuesHelper.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Infrastructure.Service.Helpers
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    
    /// <summary>
    /// Helper to manage Queues with RabbitMQ
    /// </summary>
    internal class QueuesHelper : IDisposable
    {
        private Dictionary<string, ConnectionFactory> connectionFactories;
        private Dictionary<string, IConnection> connections;
        private Dictionary<string, IModel> channels;
        IFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Instanciate a new <see cref="QueuesHelper"/>.
        /// </summary>
        public QueuesHelper()
        {
            channels = new Dictionary<string, IModel>();

            connections = new Dictionary<string, IConnection>();

            connectionFactories = new Dictionary<string, ConnectionFactory>();
        }

        /// <summary>
        /// Send Message 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="queueName"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool SendMessage(string endpoint, string queueName, object body)
        {
            IModel channel = GetChannelByEndpointAndQueueName(endpoint, queueName);

            using (MemoryStream stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                formatter.Serialize(stream, body);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                byte[] bytes = stream.ToArray();
                channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: bytes);
            }

            return true;
        }

        public void ReceiveMessage<T>(string endpoint, string queueName, Action<T> action) where T : class
        {
            var channel = GetChannelByEndpointAndQueueName(endpoint, queueName);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();

                using (MemoryStream stream = new MemoryStream(body))
                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    if (formatter.Deserialize(stream) is T result)
                    {
                        action.Invoke(result);
                    }
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                }

            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Dispose()
        {
            foreach (var channel in channels)
            {
                channel.Value.Close();
            }
            channels.Clear();

            foreach (var connection in connections)
            {
                connection.Value.Close();
            }
            connections.Clear();

            connectionFactories.Clear();
        }

        private IModel GetChannelByEndpointAndQueueName(string endpoint, string queueName)
        {
            string key = $"%_{endpoint}_{queueName}_%";

            if (channels.ContainsKey(key))
            {
                return channels[key];
            }

            IConnection connection = GetConnectionByEndpoint(endpoint);
            IModel channel = connection.CreateModel();

            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channels.Add(key, channel);

            return channel;
        }

        private IConnection GetConnectionByEndpoint(string endpoint)
        {
            if (connections.ContainsKey(endpoint))
            {
                return connections[endpoint];
            }

            ConnectionFactory connectionFactory = connectionFactories.ContainsKey(endpoint) ? connectionFactories[endpoint] : new ConnectionFactory { HostName = endpoint };
            connectionFactories.Add(endpoint, connectionFactory);

            IConnection connection = connectionFactory.CreateConnection();
            connections.Add(endpoint, connection);

            return connection;
        }
    }
}
