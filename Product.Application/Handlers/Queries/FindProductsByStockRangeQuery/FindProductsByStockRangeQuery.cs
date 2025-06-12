using FluentValidation;
using MediatR;
using Product.Domain.Entities;

namespace Product.Application.Handlers.Queries.FindProductsByStockRangeQuery;

public record FindProductsByStockRangeQuery(
    int Min,
    int Max) : IRequest<List<ProductEntity>>;

internal class FindProductsByStockRangeQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<FindProductsByStockRangeQuery, List<ProductEntity>>
{
    public async Task<List<ProductEntity>> Handle(FindProductsByStockRangeQuery request, CancellationToken cancellationToken)
    {
        if (request.Min < 0 || request.Max < 0 || request.Min > request.Max)
        {
            throw new ValidationException("Invalid Range Passed");
        }

        var products = await unitOfWork.ProductRepository.FindProductsByStockRange(request.Min, request.Max);
        return products;
    }
}