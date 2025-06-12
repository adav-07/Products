using FluentAssertions;
using FluentValidation;
using Moq;
using Product.Application;
using Product.Application.Handlers.Queries.FindProductsByStockRangeQuery;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Queries;

public class FindProductsByStockRangeQueryUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly FindProductsByStockRangeQueryHandler _handler;

    public FindProductsByStockRangeQueryUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepo.Object);

        _handler = new FindProductsByStockRangeQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenRangeIsValid()
    {
        var products = new List<ProductEntity>
        {
            new ProductEntity { Id = 1, Name = "Item1", Stock = 10 },
            new ProductEntity { Id = 2, Name = "Item2", Stock = 20 }
        };

        _mockProductRepo.Setup(r => r.FindProductsByStockRange(5, 25))
            .ReturnsAsync(products);

        var query = new FindProductsByStockRangeQuery(5, 25);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(products);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(5, -10)]
    [InlineData(20, 10)]
    public async Task Handle_ShouldThrowValidationException_WhenRangeIsInvalid(int min, int max)
    {
        var query = new FindProductsByStockRangeQuery(min, max);

        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Invalid Range Passed");
    }
}