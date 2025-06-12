namespace Product.Application.Handlers.Commands.AddProductCommand;

public record AddProductCommandDto(
    string ProductName,
    int Stock);