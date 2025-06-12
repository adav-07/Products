using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Commands.DeleteProductCommand;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Commands;

public class DeleteProductCommandUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly Mock<IProductIdPoolRepository> _mockIdPoolRepo;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockIdPoolRepo = new Mock<IProductIdPoolRepository>();

        _mockUnitOfWork.Setup(x => x.ProductRepository).Returns(_mockProductRepo.Object);
        _mockUnitOfWork.Setup(x => x.ProductIdPoolRepository).Returns(_mockIdPoolRepo.Object);

        _handler = new DeleteProductCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteProduct_AndMarkIdAvailable_WhenProductExists()
    {
        var product = new ProductEntity { Id = 123, Name = "Test", Stock = 10 };
        var command = new DeleteProductCommand(123);

        _mockProductRepo.Setup(r => r.FindProductById(123)).ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(123);

        _mockProductRepo.Verify(r => r.Delete(product), Times.Once);
        _mockIdPoolRepo.Verify(p => p.Update(It.Is<ProductIdPoolEntity>(x => x.Id == 123 && x.IsAvailable)), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        var command = new DeleteProductCommand(999);
        _mockProductRepo.Setup(r => r.FindProductById(999)).ReturnsAsync((ProductEntity?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeNull();

        _mockProductRepo.Verify(r => r.Delete(It.IsAny<ProductEntity>()), Times.Never);
        _mockIdPoolRepo.Verify(p => p.Update(It.IsAny<ProductIdPoolEntity>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}