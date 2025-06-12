using MediatR;
using Product.Application.Handlers.Commands.BaseCommand;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Commands.AddProductCommand;

public record AddProductCommand(
    string Name,
    int Stock) : BaseCommand<ProductEntity>(Stock);

internal class AddProductCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddProductCommand, ProductEntity>
{
    public async Task<ProductEntity> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var productId = await unitOfWork.ProductIdPoolRepository.GetAvailableId();

        if (productId == null)
            throw new Exception("No More Products can be inserted");

        var product = new ProductEntity
        {
            Id = productId.Id,
            Name = request.Name,
            Stock = request.Stock,
            CreatedAt = DateTime.UtcNow
        };

        productId.IsAvailable = false;

        await unitOfWork.ProductIdPoolRepository.Update(productId);
        await unitOfWork.ProductRepository.Add(product);
        await unitOfWork.CommitAsync();

        return product;
    }
}
