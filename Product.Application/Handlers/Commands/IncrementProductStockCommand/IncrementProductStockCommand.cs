using MediatR;
using Product.Application.Handlers.Commands.BaseCommand;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Commands.IncrementProductStockCommand;

public record IncrementProductStockCommand(
    int Id,
    int Stock) : BaseCommandWithId<ProductEntity?>(Id, Stock);

internal class IncrementProductStockCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<IncrementProductStockCommand, ProductEntity?>
{
    public async Task<ProductEntity?> Handle(IncrementProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.FindProductById(request.Id);

        if (product == null || request.Stock < 0)
            return null;

        product.Stock += request.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.ProductRepository.Update(product);
        await unitOfWork.CommitAsync();

        return product;
    }
}