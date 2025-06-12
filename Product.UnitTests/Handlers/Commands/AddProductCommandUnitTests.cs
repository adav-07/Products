using FluentAssertions;
using Moq;
using Product.Application;
using Product.Application.Handlers.Commands.AddProductCommand;
using Product.Application.Repositories;
using Product.Domain.Entities;

namespace Product.UnitTests.Handlers.Commands;

public class AddProductCommandUnitTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductIdPoolRepository> _mockIdPoolRepo;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly AddProductCommandHandler _handler;

    public AddProductCommandUnitTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockIdPoolRepo = new Mock<IProductIdPoolRepository>();
        _mockProductRepo = new Mock<IProductRepository>();

        _mockUnitOfWork.Setup(u => u.ProductIdPoolRepository).Returns(_mockIdPoolRepo.Object);
        _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepo.Object);

        _handler = new AddProductCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddProduct_WhenIdAvailable()
    {
        var command = new AddProductCommand("Test Product", 10);
        var idPool = new ProductIdPoolEntity { Id = 123456, IsAvailable = true };

        _mockIdPoolRepo.Setup(r => r.GetAvailableId()).ReturnsAsync(idPool);

        var result = await _handler.Handle(command, CancellationToken.None);

        _mockIdPoolRepo.Verify(r => r.Update(It.Is<ProductIdPoolEntity>(p => p.Id == 123456 && !p.IsAvailable)), Times.Once);
        _mockProductRepo.Verify(r => r.Add(It.Is<ProductEntity>(p => p.Id == 123456 && p.Name == "Test Product" && p.Stock == 10)), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);

        result.Id.Should().Be(123456);
        result.Name.Should().Be("Test Product");
        result.Stock.Should().Be(10);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenNoIdAvailable()
    {
        var command = new AddProductCommand("Test Product", 5);
        _mockIdPoolRepo.Setup(r => r.GetAvailableId()).ReturnsAsync((ProductIdPoolEntity?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No More Products can be inserted");
    }
}