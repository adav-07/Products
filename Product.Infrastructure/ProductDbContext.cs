using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;

namespace Product.Infrastructure;

public sealed class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        Product = Set<ProductEntity>();
        ProductIdPool = Set<ProductIdPoolEntity>();
    }
    public DbSet<ProductEntity> Product { get; set; }

    public DbSet<ProductIdPoolEntity> ProductIdPool { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}