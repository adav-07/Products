using Product.Application;
using Product.Application.Repositories;
using Product.Infrastructure.Repositories;

namespace Product.Infrastructure;

public class UnitOfWork(ProductDbContext context) : IUnitOfWork
{
    private IProductRepository? _productRepository;

    private IProductIdPoolRepository? _productIdPoolRepository;

    public IProductRepository ProductRepository
    {
        get
        {
            if (_productRepository != null)
                return _productRepository;

            _productRepository = new ProductRepository(context);
            return _productRepository;
        }
    }

    public IProductIdPoolRepository ProductIdPoolRepository
    {
        get
        {
            if (_productIdPoolRepository != null)
                return _productIdPoolRepository;

            _productIdPoolRepository = new ProductIdPoolRepository(context);
            return _productIdPoolRepository;
        }
    }
    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }
}