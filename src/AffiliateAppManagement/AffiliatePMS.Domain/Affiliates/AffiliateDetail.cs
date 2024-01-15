namespace AffiliatePMS.Domain.Affiliates;

public partial class AffiliateDetail
{
    public int AffiliateId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public virtual Affiliate Affiliate { get; set; } = null!;
}
