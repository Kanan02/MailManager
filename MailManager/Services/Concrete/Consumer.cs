using MailManager.Configs;
using MailManager.Services.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Text;

//using AE.Net.Mail;
using MimeKit;
using MailKit.Net.Imap;
using MailKit;

namespace MailManager.Services.Concrete
{
    public class Consumer:CronJobService
    {

        public static ImapClient IC;

        private readonly ILogger<Consumer> _logger;
        public Consumer(IScheduleConfig<Consumer> config, ILogger<Consumer> logger)
        : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Mail Consumer starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Mail Consumer is working.");
            List<MimeMessage> messages = new List<MimeMessage>();
            using (var client = new ImapClient())
            {

                using (var cancel = new CancellationTokenSource())
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate("atltaskconsumer@gmail.com", "atl12345");
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly, cancel.Token);

                    for (int i = 0; i < inbox.Count; i++)
                    {
                        if (i == 21)
                            break;
                        messages.Add(inbox.GetMessage(inbox.Count - i - 1));
                    }

                }
            }


            for (int i = 0; i < messages.Count; i++)
            {
                Console.WriteLine(messages[i].Subject);
                Console.WriteLine(messages[i].Body);
                _logger.LogWarning($"Subject: ({messages[i].Subject})  Message: ({messages[i].Body})");
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Mail Consumer is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
