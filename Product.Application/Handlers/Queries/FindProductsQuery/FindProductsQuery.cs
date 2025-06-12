using MediatR;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Queries.FindProductsQuery;

public record FindProductsQuery : IRequest<List<ProductEntity>>;

internal class FindProductsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FindProductsQuery, List<ProductEntity>>
{
    public async Task<List<ProductEntity>> Handle(FindProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await unitOfWork.ProductRepository.FindProducts();

        return products;
    }
}