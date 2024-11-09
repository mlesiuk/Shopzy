namespace Architecture.Tests;

public static class Consts
{
    public const string DomainNamespace = "Shopzy.Domain";
    public const string ApplicationNamespace = "Shopzy.Application";
    public const string InfrastructureNamespace = "Shopzy.Infrastructure";
    public const string ApiNamespace = "Shopzy.Api";

    public const string ApplicationCommandsNamespace = "Shopzy.Application.Commands";
    public const string ApplicationQueriesNamespace = "Shopzy.Application.Queries";
    public const string ApplicationValidatorsNamespace = "Shopzy.Application.Validators";

    public const string Command = nameof(Command);
    public const string Handler = nameof(Handler);
    public const string Query = nameof(Query);
    public const string Validator = nameof(Validator);
}
