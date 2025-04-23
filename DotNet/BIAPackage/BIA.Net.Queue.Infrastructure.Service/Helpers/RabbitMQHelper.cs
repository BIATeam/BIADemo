// <copyright file="RabbitMQHelper.cs" company="BIA">
//  Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Infrastructure.Service.Helpers
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using BIA.Net.Queue.Domain.Dto.Queue;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    /// <summary>
    /// Helper to manage RabbitMQ.
    /// </summary>
    internal static class RabbitMQHelper
    {
        /// <summary>
        /// Send Message to a specific endpoint.
        /// </summary>
        /// <param name="topic">The name of the topic. Example : DM.VMR.eZTest (limit to 255).</param>
        /// <param name="body">The message body.</param>
        /// <param name="user">The optional username.</param>
        /// <param name="password">The optional password.</param>
        /// <returns>The result of the sending.</returns>
        public static bool SendMessage(TopicDto topic, object body, string user = null, string password = null)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory { UserName = user, Password = password, VirtualHost = topic.VirtualHost ?? "/", HostName = topic.Endpoint, Port = AmqpTcpEndpoint.UseDefaultPort };
            using IConnection connection = connectionFactory.CreateConnection();
            using IModel channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: topic.Exchange, durable: true, type: ExchangeType.Topic);

            using MemoryStream stream = new MemoryStream();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            XmlSerializer xmlSerializer = new(body.GetType());
            xmlSerializer.Serialize(stream, body);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            byte[] bytes = stream.ToArray();
            channel.BasicPublish(
                exchange: topic.Exchange,
                routingKey: topic.RoutingKey,
                basicProperties: null,
                body: bytes);

            return true;
        }

        /// <summary>
        /// Receive a message.
        /// </summary>
        /// <typeparam name="T">The type of the body.</typeparam>
        /// <param name="topic">the global topic information to listen.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="cancellationToken">A token to cancel the receive.</param>
        /// <param name="user">The optional username.</param>
        /// <param name="password">The optional password.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public static async Task ReceiveMessageAsync<T>(TopicDto topic, Action<T> action, CancellationToken cancellationToken, string user = null, string password = null)
            where T : class
        {
            ConnectionFactory connectionFactory = new ConnectionFactory { UserName = user, Password = password, VirtualHost = topic.VirtualHost, HostName = topic.Endpoint, Port = AmqpTcpEndpoint.UseDefaultPort };
            using IConnection connection = connectionFactory.CreateConnection();
            using IModel channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: topic.Exchange, durable: true, type: ExchangeType.Topic);

            string queueName = channel.QueueDeclare(queue: $"{topic.RoutingKey}Queue", durable: true, exclusive: false, autoDelete: false).QueueName;

            EventingBasicConsumer consumer = new(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();

                using MemoryStream stream = new(body);
                XmlSerializer xmlSerializer = new(typeof(T));
                if (xmlSerializer.Deserialize(stream) is T result)
                {
                    action.Invoke(result);
                }
            };

            channel.QueueBind(
                queue: queueName,
                exchange: topic.Exchange,
                routingKey: topic.RoutingKey);

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }
}
