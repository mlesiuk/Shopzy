using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Shopzy.Api.Services;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Configurations;

namespace Shopzy.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddSingleton<ICurrentUserService, CurrentUserService>();

        return services;
    }

    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var elasticCfg = new ElasticsearchConfiguration();
        configuration.GetSection(nameof(Configuration.Elasticsearch)).Bind(elasticCfg);
        
        hostBuilder.UseSerilog((hostBuilder, loggerConfiguration) =>
        {
            var envName = hostBuilder.HostingEnvironment.EnvironmentName.ToLower().Replace(".", "-");
            var template = $"{elasticCfg.IndexTemplate}-{envName}";
            var username = elasticCfg.Username;
            var password = elasticCfg.Password;
            var url = elasticCfg.Url;

            loggerConfiguration
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(url))
                {
                    IndexFormat = $"{template}-{envName}",
                    AutoRegisterTemplate = true,
                    TemplateName = template,
                    BatchAction = ElasticOpType.Create,
                    ModifyConnectionSettings = configuration =>
                    {
                        configuration.BasicAuthentication(username, password);
                        configuration.ServerCertificateValidationCallback((o, certificate, arg3, arg4) =>
                        {
                            return true;
                        });
                        return configuration;
                    }
                })
                .ReadFrom.Configuration(configuration);
        });
        return hostBuilder;
    }
}
