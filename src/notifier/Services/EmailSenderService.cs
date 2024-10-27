using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Shopzy.Notifier.Abstractions.Interfaces;
using Shopzy.Notifier.Configurations;
using Shopzy.Notifier.Models;

namespace Shopzy.Notifier.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailSenderConfiguration _emailSenderConfiguration;

    public EmailSenderService(IOptions<EmailSenderConfiguration> options)
    {
        _emailSenderConfiguration = options.Value;
    }

    public async Task SendMailAsync(MailContext mailContext, CancellationToken cancellationToken = default)
    {
        IEnumerable<MailboxAddress> fromMailboxAddresses = mailContext.FromAddresses
            .Select(x => new MailboxAddress(x.Key, x.Value)).ToList();

        IEnumerable<MailboxAddress> toMailboxAddresses = mailContext.ToAddresses
            .Select(x => new MailboxAddress(x.Key, x.Value)).ToList();

        var message = new MimeMessage();
        message.From.AddRange(fromMailboxAddresses);
        message.To.AddRange(toMailboxAddresses);
        message.Subject = mailContext.Subject;

        message.Body = new TextPart("plain")
        {
            Text = mailContext.Body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSenderConfiguration.Host, _emailSenderConfiguration.Port, false, cancellationToken);
        await client.AuthenticateAsync(_emailSenderConfiguration.Username, _emailSenderConfiguration.Password, cancellationToken);
        _ = await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
