using MediatR;
using Product.Application.Handlers.Commands.BaseCommand;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Commands.EditProductCommand;

public record EditProductCommand(
    int Id,
    string Name,
    int Stock) : BaseCommandWithId<ProductEntity?>(Id, Stock);

internal class EditProductCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<EditProductCommand, ProductEntity?>
{
    public async Task<ProductEntity?> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.FindProductById(request.Id);

        if (product == null)
            return product;

        product.Name = request.Name;
        product.Stock = request.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.ProductRepository.Update(product);
        await unitOfWork.CommitAsync();

        return product;
    }
}