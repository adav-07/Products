using Microsoft.EntityFrameworkCore;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.Infrastructure.Repositories;

public class ProductIdPoolRepository(ProductDbContext context) : IProductIdPoolRepository
{
    public async Task<ProductIdPoolEntity?> GetAvailableId()
    {
        var productPool = await context.ProductIdPool.Where(x => x.IsAvailable).FirstOrDefaultAsync();

        return productPool;
    }

    public Task Update(ProductIdPoolEntity productIdPoolEntity)
    {
        context.ProductIdPool.Update(productIdPoolEntity);
        return Task.CompletedTask;
    }
}