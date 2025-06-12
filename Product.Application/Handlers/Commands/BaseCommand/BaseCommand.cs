using MediatR;

namespace Product.Application.Handlers.Commands.BaseCommand;

public record BaseCommand<TResponse>(
    int Stock) : IRequest<TResponse>;