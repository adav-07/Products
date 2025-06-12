using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Product.Infrastructure.Factories;

public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings"));

        return new ProductDbContext(optionsBuilder.Options);
    }
}