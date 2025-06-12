using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Commands.DecrementProductStockCommand;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Commands;

public class DecrementProductStockCommandUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly DecrementProductStockCommandHandler _handler;

    public DecrementProductStockCommandUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();

        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepo.Object);

        _handler = new DecrementProductStockCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDecrementStock_WhenValidRequest()
    {
        var product = new ProductEntity { Id = 1, Name = "Test", Stock = 10 };
        var command = new DecrementProductStockCommand(1, 4);

        _mockProductRepo.Setup(r => r.FindProductById(1)).ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Stock.Should().Be(6); // 10 - 4

        _mockProductRepo.Verify(r => r.Update(It.Is<ProductEntity>(p => p.Id == 1 && p.Stock == 6)), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Theory]
    [InlineData(null, 3)] // product not found
    [InlineData(10, -1)] // negative stock decrement
    [InlineData(2, 5)] // stock too low
    public async Task Handle_ShouldReturnNull_WhenInvalid(int? existingStock, int decrement)
    {
        var command = new DecrementProductStockCommand(1, decrement);

        ProductEntity? product = existingStock is null
            ? null
            : new ProductEntity { Id = 1, Name = "Test", Stock = existingStock.Value };

        _mockProductRepo.Setup(r => r.FindProductById(1)).ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeNull();
        _mockProductRepo.Verify(r => r.Update(It.IsAny<ProductEntity>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}