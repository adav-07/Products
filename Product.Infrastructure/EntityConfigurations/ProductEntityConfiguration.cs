using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Infrastructure.EntityConfigurations;

internal class ProductEntityConfiguration : BaseEntityConfiguration<ProductEntity>
{
    public override void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}