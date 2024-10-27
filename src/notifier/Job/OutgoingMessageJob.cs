using Microsoft.EntityFrameworkCore;
using Quartz;
using Shopzy.Notifier.Abstractions.Interfaces;
using Shopzy.Notifier.Models;
using Shopzy.Notifier.Persistence;
using System.Text.Json;

namespace Shopzy.Notifier.Job;

[DisallowConcurrentExecution]
public class OutgoingMessageJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailSenderService _emailSenderService;

    public OutgoingMessageJob(
        ApplicationDbContext dbContext,
        IEmailSenderService emailSenderService)
    {
        _dbContext = dbContext;
        _emailSenderService = emailSenderService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var outgoingMessages = await _dbContext.OutgoingMessages
            .Where(m => m.ProcessedAt == null)
            .Take(20)
            .OrderBy(m => m.RegisteredAt)
            .ToListAsync(context.CancellationToken);

        foreach (var outgoingMessage in outgoingMessages)
        {
            if (outgoingMessage is null)
            {
                continue;
            }

            if (outgoingMessage.Content is null)
            {
                continue;
            }

            var mailContext = JsonSerializer.Deserialize<MailContext>(outgoingMessage.Content);
            if (mailContext is null)
            {
                continue;
            }
            await _emailSenderService.SendMailAsync(mailContext, context.CancellationToken);
            outgoingMessage.ProcessedAt = DateTime.UtcNow;
        }

        if (outgoingMessages.Count > 0)
        {
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
