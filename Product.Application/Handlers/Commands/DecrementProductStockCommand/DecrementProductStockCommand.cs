using MediatR;
using Product.Application.Handlers.Commands.BaseCommand;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Commands.DecrementProductStockCommand;

public record DecrementProductStockCommand(
    int Id,
    int Stock) : BaseCommandWithId<ProductEntity?>(Id, Stock);

internal class DecrementProductStockCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DecrementProductStockCommand, ProductEntity?>
{
    public async Task<ProductEntity?> Handle(DecrementProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.FindProductById(request.Id);

        if (product == null || request.Stock < 0 || product.Stock < request.Stock)
            return null;

        product.Stock -= request.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.ProductRepository.Update(product);
        await unitOfWork.CommitAsync();

        return product;
    }
}