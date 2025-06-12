using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Queries.FindProductsByNameQuery;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Queries;

public class FindProductsByNameQueryUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly FindProductsByNameQueryHandler _handler;

    public FindProductsByNameQueryUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepo.Object);

        _handler = new FindProductsByNameQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMatchingProducts_WhenNameMatches()
    {
        var products = new List<ProductEntity>
        {
            new ProductEntity { Id = 1, Name = "Apple", Stock = 10 },
            new ProductEntity { Id = 2, Name = "Pineapple", Stock = 5 },
            new ProductEntity { Id = 3, Name = "Banana", Stock = 15 }
        };

        _mockProductRepo.Setup(r => r.FindProducts()).ReturnsAsync(products);

        var query = new FindProductsByNameQuery("apple");
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(p => p.Name).Should().Contain(new[] { "Apple", "Pineapple" });
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoMatch()
    {
        var products = new List<ProductEntity>
        {
            new ProductEntity { Id = 1, Name = "Apple", Stock = 10 },
            new ProductEntity { Id = 2, Name = "Banana", Stock = 20 }
        };

        _mockProductRepo.Setup(r => r.FindProducts()).ReturnsAsync(products);

        var query = new FindProductsByNameQuery("orange");
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}