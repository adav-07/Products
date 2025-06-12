namespace Product.Domain.Entities;

public class ProductEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
}