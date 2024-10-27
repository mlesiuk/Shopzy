using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Shopzy.Application.Behaviours;
using Shopzy.Application.Common;
using Shopzy.Application.Configurations;
using System.Reflection;

namespace Shopzy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddJwtAuthentication(configuration);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR();

        var redisCfg = new RedisConfiguration();
        configuration.GetSection(nameof(Configuration.Redis)).Bind(redisCfg);

        var rabbitMqCfg = new RabbitMqConfiguration();
        configuration.GetSection(nameof(Configuration.RabbitMq)).Bind(rabbitMqCfg);

        services.AddMassTransit(configuration =>
        {
            configuration.SetKebabCaseEndpointNameFormatter();

            configuration.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
                cfg.Host(new Uri(rabbitMqCfg.Uri), rmhc =>
                {
                    rmhc.Username(rabbitMqCfg.Username);
                    rmhc.Password(rabbitMqCfg.Password);
                });
            });
        });

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.TryAddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection(nameof(Configuration.Jwt));
        services.AddOptions<JwtConfiguration>()
            .Bind(jwtSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtConfiguration = new JwtConfiguration();
        jwtSection.Bind(jwtConfiguration);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Convert.FromBase64String(jwtConfiguration.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Consts.Administrator,
                policy =>
                {
                    policy.RequireClaim(CustomJwtRegisteredClaimNames.AssignedRoles, Consts.Administrator);
                });
        });

        return services;
    }
}
