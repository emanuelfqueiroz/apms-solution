using AffiliatePMS.Domain.AffiliateCustomers;

namespace AffiliatePMS.Domain.Sales;

public partial class OrderHeader
{
    public Guid Id { get; set; }

    public int? AffiliateCustomerId { get; set; }

    public decimal TotalAmountItems { get; set; }

    public virtual AffiliateCustomer? AffiliateCustomer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
