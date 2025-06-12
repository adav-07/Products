using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Queries.FindProductsQuery;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Queries;

public class FindProductsQueryUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly FindProductsQueryHandler _handler;

    public FindProductsQueryUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new FindProductsQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllProducts()
    {
        var expectedProducts = new List<ProductEntity>
        {
            new ProductEntity { Id = 1, Name = "Product A", Stock = 10 },
            new ProductEntity { Id = 2, Name = "Product B", Stock = 20 }
        };

        _mockProductRepository
            .Setup(r => r.FindProducts())
            .ReturnsAsync(expectedProducts);

        var result = await _handler.Handle(new FindProductsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(expectedProducts);
    }
}