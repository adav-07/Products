using MediatR;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Commands.DeleteProductCommand;

public record DeleteProductCommand(
    int Id) : IRequest<ProductEntity?>;

internal class DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductCommand, ProductEntity?>
{
    public async Task<ProductEntity?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.FindProductById(request.Id);

        if (product == null)
            return null;


        await unitOfWork.ProductRepository.Delete(product);
        await unitOfWork.ProductIdPoolRepository.Update(new ProductIdPoolEntity
        {
            Id = request.Id,
            IsAvailable = true
        });
        await unitOfWork.CommitAsync();

        return product;
    }
}