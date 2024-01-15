using AffiliatePMS.Domain.Finance;

namespace AffiliatePMS.Domain.Affiliates;

public partial class Affiliate
{
    public int Id { get; set; }

    public string? PublicName { get; set; }

    public virtual AffiliateAddress? AffiliateAddress { get; set; }

    public virtual AffiliateBankAccount? AffiliateBankAccount { get; set; }

    public virtual ICollection<AffiliateCustomers.AffiliateCustomer> AffiliateCustomers { get; set; } = new List<AffiliateCustomers.AffiliateCustomer>();

    public virtual AffiliateDetail? AffiliateDetail { get; set; }

    public virtual ICollection<AffiliateSocialMedia> AffiliateSocialMedia { get; set; } = new List<AffiliateSocialMedia>();
    public int? UserCreatedId { get; set; }
}
