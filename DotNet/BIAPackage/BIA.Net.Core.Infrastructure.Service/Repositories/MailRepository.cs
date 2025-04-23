// <copyright file="MailRepository.cs" company="BIA">
//  Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.RepoContract;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using MimeKit.Text;

    /// <summary>
    /// The class representing a NotificationRepository.
    /// </summary>
    public class MailRepository : IMailRepository
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MailRepository(IOptions<BiaNetSection> configuration)
        {
            this.configuration = configuration.Value;
        }

        /// <summary>
        /// Send Notification.
        /// </summary>
        /// <param name="subject">the subject.</param>
        /// <param name="bodyText">the body of the notification.</param>
        /// <param name="tos">list of destinator.</param>
        /// <param name="ccs">list of copy.</param>
        /// <returns>the async task.</returns>
        public async Task SendNotificationAsync(string subject, string bodyText, IEnumerable<string> tos, IEnumerable<string> ccs = null)
        {
            MimeMessage messageToSend = new ()
            {
                Subject = subject?.Trim(),
            };

            messageToSend.From.Add(new MailboxAddress(this.configuration.EmailConfiguration.From, this.configuration.EmailConfiguration.From));

            if (tos?.Any() == true)
            {
                foreach (string to in tos)
                {
                    messageToSend.To.Add(new MailboxAddress(to?.Trim(), to?.Trim()));
                }
            }

            if (ccs?.Any() == true)
            {
                foreach (string cc in ccs)
                {
                    messageToSend.Cc.Add(new MailboxAddress(cc?.Trim(), cc?.Trim()));
                }
            }

            bodyText = $"<div style='font-family:Calibri'>{bodyText}</div>";

            messageToSend.Body = new TextPart(TextFormat.Html) { Text = bodyText?.Trim() };

            using (SmtpClient client = new ())
            {
                await client.ConnectAsync(this.configuration.EmailConfiguration.SmtpHost, this.configuration.EmailConfiguration.SmtpPort, false);
                await client.SendAsync(messageToSend);
                await client.DisconnectAsync(true);
            }
        }
    }
}
