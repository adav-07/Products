using MediatR;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Queries.FindProductByIdQuery;

public record FindProductByIdQuery(int ProductId) : IRequest<ProductEntity?>;

internal class FindProductByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FindProductByIdQuery, ProductEntity?>
{
    public async Task<ProductEntity?> Handle(FindProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.FindProductById(request.ProductId);

        return product;
    }
}