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
    using System.Net;
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
        /// Send Message to a specific endpoint.
        /// </summary>
        /// <param name="endpoint">The message endpoint destination.</param>
        /// <param name="queueName">The name of the destination queue.</param>
        /// <param name="body">The message body.</param>
        /// <param name="user">The optional username.</param>
        /// <param name="password">The optional password.</param>
        /// <returns>The result of the sending.</returns>
        public bool SendMessage(string endpoint, string queueName, object body, string user = null, string password = null)
        {
            IModel channel = GetChannelByEndpointAndQueueName(endpoint, queueName, user, password);

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

        /// <summary>
        /// Recieve a message.
        /// </summary>
        /// <typeparam name="T">The type of the body.</typeparam>
        /// <param name="endpoint">The endpoint to listen.</param>
        /// <param name="queueName">the queue nameto listen.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="user">The optional username.</param>
        /// <param name="password">The optional password.</param>
        public void ReceiveMessage<T>(string endpoint, string queueName, Action<T> action, string user = null, string password = null) where T : class
        {
            var channel = GetChannelByEndpointAndQueueName(endpoint, queueName, user, password);

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

        private IModel GetChannelByEndpointAndQueueName(string endpoint, string queueName, string user, string password)
        {
            string key = $"%_{endpoint}_{queueName}_{user ?? "guest"}_%";

            if (channels.ContainsKey(key))
            {
                return channels[key];
            }

            IConnection connection = GetConnectionByEndpoint(endpoint, user, password);
            IModel channel = connection.CreateModel();

            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channels.Add(key, channel);

            return channel;
        }

        private IConnection GetConnectionByEndpoint(string endpoint, string user, string password)
        {
            string key = $"_{endpoint}_{user ?? "guest"}_";

            if (connections.ContainsKey(key))
            {
                return connections[key];
            }

            ConnectionFactory connectionFactory = connectionFactories.ContainsKey(key) ? connectionFactories[key] :
                user == null ? new ConnectionFactory { HostName = endpoint, Port = AmqpTcpEndpoint.UseDefaultPort} :
                new ConnectionFactory
                {
                    UserName = user,
                    Password = password,
                    VirtualHost = "/",
                    HostName = endpoint,
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                };
            connectionFactories.Add(key, connectionFactory);

            IConnection connection = connectionFactory.CreateConnection();
            connections.Add(key, connection);

            return connection;
        }
    }
}
