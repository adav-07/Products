using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Commands.EditProductCommand;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Commands;

public class EditProductCommandUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly EditProductCommandHandler _handler;

    public EditProductCommandUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepo.Object);
        _handler = new EditProductCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenProductExists()
    {
        var product = new ProductEntity { Id = 1, Name = "Old", Stock = 5 };
        var command = new EditProductCommand(1, "NewName", 20);

        _mockProductRepo.Setup(r => r.FindProductById(1)).ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("NewName");
        result.Stock.Should().Be(20);
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        _mockProductRepo.Verify(r => r.Update(product), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        var command = new EditProductCommand(2, "Name", 10);
        _mockProductRepo.Setup(r => r.FindProductById(2)).ReturnsAsync((ProductEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeNull();

        _mockProductRepo.Verify(r => r.Update(It.IsAny<ProductEntity>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}