using Microsoft.Extensions.DependencyInjection;
using Product.Application;

namespace Product.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}