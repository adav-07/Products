using Product.Domain.Entities;

namespace Product.Application.Repositories;

public interface IProductRepository
{
    Task<List<ProductEntity>> FindProducts();

    Task<ProductEntity?> FindProductById(int id);

    Task<List<ProductEntity>> FindProductsByStockRange(int min, int max);

    Task Add(ProductEntity product);

    Task Update(ProductEntity product);

    Task Delete(ProductEntity product);
}