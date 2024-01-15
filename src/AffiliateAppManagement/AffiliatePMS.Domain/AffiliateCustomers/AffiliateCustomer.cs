using AffiliatePMS.Domain.Affiliates;
using AffiliatePMS.Domain.Sales;

namespace AffiliatePMS.Domain.AffiliateCustomers;

public partial class AffiliateCustomer
{
    public int Id { get; set; }

    public int? AffiliateId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public decimal? TotalPurchase { get; set; }

    public decimal? AvgTicket { get; set; }

    public virtual Affiliate? Affiliate { get; set; }

    public virtual ICollection<OrderHeader> OrderHeaders { get; set; } = new List<OrderHeader>();
}
