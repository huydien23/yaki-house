using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Kitchen.Services;
using Yakihouse.Domain.Repositories;
using Yakihouse.Infrastructure.Persistence;
using Yakihouse.Infrastructure.Repositories;
using Yakihouse.Infrastructure.Services;

namespace Yakihouse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        services.AddDbContext<YakihouseDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(YakihouseDbContext).Assembly.FullName);
            });
        });

        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register query services
        services.AddScoped<IOrderQueryService, OrderQueryService>();
        services.AddScoped<IKitchenTicketService, KitchenTicketService>();

        return services;
    }
}

