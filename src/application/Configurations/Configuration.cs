using System.ComponentModel.DataAnnotations;

namespace Shopzy.Application.Configurations;

public sealed class Configuration
{
    public ElasticsearchConfiguration Elasticsearch { get; init; } = new();
    public JwtConfiguration Jwt { get; init; } = new();
    public RabbitMqConfiguration RabbitMq { get; init; } = new();
    public RedisConfiguration Redis { get; init; } = new();
}

public sealed class ElasticsearchConfiguration
{
    public string IndexTemplate { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed class JwtConfiguration
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Parameter 'Audience' is required")]
    public string Audience { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Parameter 'Issuer' is required")]
    public string Issuer { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Parameter 'Key' is required")]
    public string Key { get; init; } = string.Empty;
}

public sealed class RabbitMqConfiguration
{
    public string Uri { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed class RedisConfiguration
{
    public string ConnectionString { get; init; } = string.Empty;
}
