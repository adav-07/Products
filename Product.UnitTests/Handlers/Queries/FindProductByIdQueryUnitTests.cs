using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Queries.FindProductByIdQuery;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Queries;

public class FindProductByIdQueryUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly FindProductByIdQueryHandler _handler;

    public FindProductByIdQueryUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepo.Object);

        _handler = new FindProductByIdQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        var product = new ProductEntity { Id = 1, Name = "Test Product", Stock = 10 };

        _mockProductRepo.Setup(r => r.FindProductById(1)).ReturnsAsync(product);

        var result = await _handler.Handle(new FindProductByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        _mockProductRepo.Setup(r => r.FindProductById(99)).ReturnsAsync((ProductEntity?)null);

        var result = await _handler.Handle(new FindProductByIdQuery(99), CancellationToken.None);

        result.Should().BeNull();
    }
}