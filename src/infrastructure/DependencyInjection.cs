using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Infrastructure.Data;
using Shopzy.Infrastructure.Encryption;
using Shopzy.Infrastructure.Jwt;
using Shopzy.Infrastructure.Persistence;
using Shopzy.Infrastructure.Repositories;

namespace Shopzy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Shopzy"),
            b =>
            {
                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                b.MigrationsHistoryTable(
                    tableName: HistoryRepository.DefaultTableName,
                    schema: "dbo");
            })
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.Decorate<ICategoryRepository, CachedCategoryRepository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IEncryptor, Encryptor>();

        return services;
    }
}
