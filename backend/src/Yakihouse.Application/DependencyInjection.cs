using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Yakihouse.Application.Billing.Services;
using Yakihouse.Application.Promotions.Services;

namespace Yakihouse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Register application services
        services.AddScoped<IBillCalculationService, BillCalculationService>();
        services.AddScoped<IPromotionService, PromotionService>();
        
        return services;
    }
}

