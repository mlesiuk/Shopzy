using Shopzy.Notifier.Models;

namespace Shopzy.Notifier.Abstractions.Interfaces;

public interface IEmailSenderService
{
    Task SendMailAsync(MailContext mailContext, CancellationToken cancellationToken = default);
}
