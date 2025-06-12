using Product.Application.Repositories;

namespace Product.Application;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }

    IProductIdPoolRepository ProductIdPoolRepository { get; }
    Task CommitAsync();
}