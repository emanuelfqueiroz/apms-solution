namespace AffiliatePMS.Domain.Sales;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
