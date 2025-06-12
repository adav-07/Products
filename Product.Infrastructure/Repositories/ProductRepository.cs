using Microsoft.EntityFrameworkCore;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.Infrastructure.Repositories;

internal class ProductRepository(ProductDbContext context) : IProductRepository
{
    public async Task<List<ProductEntity>> FindProducts()
    {
        var products = await context.Product.ToListAsync();
        return products;
    }

    public async Task<ProductEntity?> FindProductById(int id)
    {
        var product = await context.Product.FirstOrDefaultAsync(x => x.Id == id);
        return product;
    }

    public async Task<List<ProductEntity>> FindProductsByStockRange(int min, int max)
    {
        var products = await context.Product.Where(x => x.Stock >= min && x.Stock <= max).ToListAsync();
        return products;
    }

    public Task Add(ProductEntity product)
    {
        context.Product.Add(product);
        return Task.CompletedTask;
    }

    public Task Update(ProductEntity product)
    {
        context.Product.Update(product);
        return Task.CompletedTask;
    }

    public Task Delete(ProductEntity product)
    {
        context.Product.Remove(product);
        return Task.CompletedTask;
    }
}