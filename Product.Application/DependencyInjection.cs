using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Handlers.Commands.AddProductCommand;
using Product.Application.Handlers.Commands.DecrementProductStockCommand;
using Product.Application.Handlers.Commands.EditProductCommand;
using Product.Application.Handlers.Commands.IncrementProductStockCommand;
using Product.Application.Validators;
using Product.Domain.Entities;

namespace Product.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProductApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IValidator<AddProductCommand>, StockValidator<AddProductCommand, ProductEntity>>();
        services.AddScoped<IValidator<EditProductCommand>, StockAndIdValidator<EditProductCommand, ProductEntity?>>();
        services.AddScoped<IValidator<IncrementProductStockCommand>, StockAndIdValidator<IncrementProductStockCommand, ProductEntity?>>();
        services.AddScoped<IValidator<DecrementProductStockCommand>, StockAndIdValidator<DecrementProductStockCommand, ProductEntity?>>();

        return services;
    }
}