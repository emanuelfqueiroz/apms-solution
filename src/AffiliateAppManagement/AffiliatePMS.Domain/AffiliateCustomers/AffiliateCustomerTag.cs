using AffiliatePMS.Domain.Affiliates;

namespace AffiliatePMS.Domain.AffiliateCustomers;

public partial class AffiliateCustomerTag
{
    public int? AffiliateId { get; set; }

    public string? Tag { get; set; }

    public short? Weigth { get; set; }

    public virtual Affiliate? Affiliate { get; set; }
}
