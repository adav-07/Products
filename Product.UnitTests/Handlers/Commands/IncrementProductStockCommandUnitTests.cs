using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Commands.IncrementProductStockCommand;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Commands;

public class IncrementProductStockCommandUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly IncrementProductStockCommandHandler _handler;

    public IncrementProductStockCommandUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepo.Object);

        _handler = new IncrementProductStockCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldIncrementStock_WhenProductExists_AndStockIsValid()
    {
        var product = new ProductEntity { Id = 1, Name = "Item", Stock = 10 };
        var command = new IncrementProductStockCommand(1, 5);

        _mockProductRepo.Setup(r => r.FindProductById(1)).ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Stock.Should().Be(15);
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        _mockProductRepo.Verify(r => r.Update(product), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        var command = new IncrementProductStockCommand(2, 5);

        _mockProductRepo.Setup(r => r.FindProductById(2)).ReturnsAsync((ProductEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeNull();
        _mockProductRepo.Verify(r => r.Update(It.IsAny<ProductEntity>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenStockIsNegative()
    {
        var product = new ProductEntity { Id = 3, Name = "Item", Stock = 20 };
        var command = new IncrementProductStockCommand(3, -1);

        _mockProductRepo.Setup(r => r.FindProductById(3)).ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeNull();
        _mockProductRepo.Verify(r => r.Update(It.IsAny<ProductEntity>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}