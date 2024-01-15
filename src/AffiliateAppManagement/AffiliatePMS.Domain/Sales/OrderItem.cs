namespace AffiliatePMS.Domain.Sales;

public partial class OrderItem
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual OrderHeader? Order { get; set; }

    public virtual Product? Product { get; set; }
}
