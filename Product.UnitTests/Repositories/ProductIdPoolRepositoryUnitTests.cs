using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Infrastructure;
using Product.Infrastructure.Repositories;

namespace Product.UnitTests.Repositories;

public class ProductIdPoolRepositoryTests
{
    private ProductDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        return new ProductDbContext(options);
    }

    [Fact]
    public async Task GetAvailableId_ReturnsAvailableId()
    {
        using var context = CreateInMemoryContext();
        context.ProductIdPool.AddRange(
            new ProductIdPoolEntity { Id = 100001, IsAvailable = false },
            new ProductIdPoolEntity { Id = 100002, IsAvailable = true }
        );
        await context.SaveChangesAsync();

        var repo = new ProductIdPoolRepository(context);
        var result = await repo.GetAvailableId();

        Assert.NotNull(result);
        Assert.Equal(100002, result.Id);
    }

    [Fact]
    public async Task GetAvailableId_ReturnsNull_WhenNoneAvailable()
    {
        using var context = CreateInMemoryContext();
        context.ProductIdPool.Add(new ProductIdPoolEntity { Id = 100001, IsAvailable = false });
        await context.SaveChangesAsync();

        var repo = new ProductIdPoolRepository(context);
        var result = await repo.GetAvailableId();

        Assert.Null(result);
    }

    [Fact]
    public async Task Update_UpdatesEntity()
    {
        using var context = CreateInMemoryContext();
        var entity = new ProductIdPoolEntity { Id = 100003, IsAvailable = true };
        context.ProductIdPool.Add(entity);
        await context.SaveChangesAsync();

        entity.IsAvailable = false;
        var repo = new ProductIdPoolRepository(context);
        await repo.Update(entity);
        await context.SaveChangesAsync();

        var updated = await context.ProductIdPool.FindAsync(100003);
        Assert.False(updated.IsAvailable);
    }
}