namespace AffiliatePMS.Domain.Affiliates;

public partial class AffiliateSocialMedia
{
    public int Id { get; set; }

    public int? AffiliateId { get; set; }

    public string? Url { get; set; }

    public string? Type { get; set; }

    public int? Followers { get; set; }

    public virtual Affiliate? Affiliate { get; set; }
}
