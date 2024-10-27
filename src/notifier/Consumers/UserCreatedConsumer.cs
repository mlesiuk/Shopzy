using MassTransit;
using Shopzy.Contracts.Events;
using Shopzy.Notifier.Models;
using Shopzy.Notifier.Persistence;
using System.Text.Json;

namespace Shopzy.Notifier.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(
        ApplicationDbContext dbContext,
        ILogger<UserCreatedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var mailContext = new MailContext
        {
            FromAddresses = new Dictionary<string, string>
            {
                { "Shopzy Team", "no-reply@shopzy.com" }
            },
            ToAddresses = new Dictionary<string, string>
            {
                { context.Message.UserName, context.Message.Email }
            },
            Subject = "User created",
            Body = $@"Hey {context.Message.UserName},

I just wanted to let you know that your account has been created!

-- Shopzy Team"
        };

        _logger.LogInformation("Mail context: {mailContext}.", mailContext);

        var outgoingMessage = new OutgoingMessage
        {
            Content = JsonSerializer.Serialize(mailContext)
        };

        await _dbContext.Set<OutgoingMessage>().AddAsync(outgoingMessage);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
