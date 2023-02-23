namespace BIA.Net.Queue.Domain.Dto.Queue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Dto to define address of RabbitMQ server and queue.
    /// </summary>
    public class QueueDto
    {
        /// <summary>
        /// The RabbitMQ server URI.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The rabbitMQ Queue to listen
        /// </summary>
        public string QueueName { get; set; }
    }
}
