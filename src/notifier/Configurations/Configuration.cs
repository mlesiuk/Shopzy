namespace Shopzy.Notifier.Configurations;

public sealed class Configuration
{
    public EmailSenderConfiguration EmailSender { get; init; } = new();
    public RabbitMqConfiguration RabbitMq { get; init; } = new();
}

public sealed class EmailSenderConfiguration
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
}

public sealed class RabbitMqConfiguration
{
    public string Uri { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
