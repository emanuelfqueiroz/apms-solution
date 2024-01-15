namespace AffiliatePMS.Domain.Affiliates;

public partial class AffiliateAddress
{
    public int AffiliateId { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? ZipCode { get; set; }

    public virtual Affiliate Affiliate { get; set; } = null!;
}
