using Product.Domain.Entities;
using Product.Infrastructure.Repositories;

namespace Product.UnitTests.Repositories;

using Microsoft.EntityFrameworkCore;
using Product.Infrastructure;
using Xunit;

public class ProductRepositoryTests
{
    private ProductDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ProductDbContext(options);
    }

    [Fact]
    public async Task Add_Product_SuccessfullyAddsProduct()
    {
        using var context = CreateInMemoryContext();
        var repo = new ProductRepository(context);
        var product = new ProductEntity { Id = 1, Name = "Test", Stock = 10 };

        await repo.Add(product);
        await context.SaveChangesAsync();

        var saved = await context.Product.FindAsync(1);
        Assert.NotNull(saved);
        Assert.Equal("Test", saved.Name);
    }

    [Fact]
    public async Task FindProductById_ReturnsCorrectProduct()
    {
        using var context = CreateInMemoryContext();
        var product = new ProductEntity { Id = 1, Name = "Test", Stock = 10 };
        context.Product.Add(product);
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        var result = await repo.FindProductById(1);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task FindProductsByStockRange_ReturnsCorrectProducts()
    {
        using var context = CreateInMemoryContext();
        context.Product.AddRange(
            new ProductEntity { Id = 1, Name = "A", Stock = 5 },
            new ProductEntity { Id = 2, Name = "B", Stock = 15 },
            new ProductEntity { Id = 3, Name = "C", Stock = 25 }
        );
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        var results = await repo.FindProductsByStockRange(10, 20);

        Assert.Single(results);
        Assert.Equal("B", results[0].Name);
    }

    [Fact]
    public async Task Update_Product_SuccessfullyUpdates()
    {
        using var context = CreateInMemoryContext();
        var product = new ProductEntity { Id = 1, Name = "Old", Stock = 5 };
        context.Product.Add(product);
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        product.Name = "New";
        await repo.Update(product);
        await context.SaveChangesAsync();

        var updated = await context.Product.FindAsync(1);
        Assert.Equal("New", updated.Name);
    }

    [Fact]
    public async Task Delete_Product_RemovesProduct()
    {
        using var context = CreateInMemoryContext();
        var product = new ProductEntity { Id = 1, Name = "Test", Stock = 10 };
        context.Product.Add(product);
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        await repo.Delete(product);
        await context.SaveChangesAsync();

        var deleted = await context.Product.FindAsync(1);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task FindProducts_ReturnsAllProducts()
    {
        using var context = CreateInMemoryContext();
        context.Product.AddRange(
            new ProductEntity { Id = 1, Name = "A", Stock = 5 },
            new ProductEntity { Id = 2, Name = "B", Stock = 10 }
        );
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        var products = await repo.FindProducts();

        Assert.Equal(2, products.Count);
    }
}
