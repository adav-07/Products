using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Product.Application.Repositories;
using Product.Domain.Entities;
using Product.Infrastructure;

namespace Product.UnitTests;

public class UnitOfWorkUnitTests
{
    private readonly ProductDbContext _context;
    private readonly UnitOfWork _unitOfWork;

    public UnitOfWorkUnitTests()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "UnitOfWorkTestDb")
            .Options;

        _context = new ProductDbContext(options);
        _unitOfWork = new UnitOfWork(_context);
    }

    [Fact]
    public void ProductRepository_ShouldReturnInstance()
    {
        var repo = _unitOfWork.ProductRepository;

        repo.Should().NotBeNull();
        repo.Should().BeAssignableTo<IProductRepository>();
    }

    [Fact]
    public void ProductIdPoolRepository_ShouldReturnInstance()
    {
        var repo = _unitOfWork.ProductIdPoolRepository;

        repo.Should().NotBeNull();
        repo.Should().BeAssignableTo<IProductIdPoolRepository>();
    }

    [Fact]
    public async Task CommitAsync_ShouldSaveChanges()
    {
        _context.Product.Add(new ProductEntity { Id = 100001, Name = "Test", Stock = 10 });

        await _unitOfWork.CommitAsync();

        var saved = await _context.Product.FirstOrDefaultAsync(x => x.Id == 100001);
        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Test");
    }
}