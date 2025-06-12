using Product.Domain.Entities;

namespace Product.Application.Repositories;

public interface IProductIdPoolRepository
{
    Task<ProductIdPoolEntity?> GetAvailableId();

    Task Update(ProductIdPoolEntity productIdPoolEntity);

}