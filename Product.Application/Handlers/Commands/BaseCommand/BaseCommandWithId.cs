namespace Product.Application.Handlers.Commands.BaseCommand;

public record BaseCommandWithId<TResponse>(
    int Id,
    int Stock) : BaseCommand<TResponse>(Stock);