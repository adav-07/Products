using MediatR;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Queries.FindProductsByNameQuery;

public record FindProductsByNameQuery(
    string Name) : IRequest<List<ProductEntity>>;

internal class FindProductsByNameQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FindProductsByNameQuery, List<ProductEntity>>
{
    public async Task<List<ProductEntity>> Handle(FindProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var products = await unitOfWork.ProductRepository.FindProducts();

        var result = products
            .Where(p => p.Name.Contains(request.Name.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();

        return result;
    }
}