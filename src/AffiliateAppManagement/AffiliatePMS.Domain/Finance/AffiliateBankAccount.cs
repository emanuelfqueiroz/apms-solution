using AffiliatePMS.Domain.Affiliates;

namespace AffiliatePMS.Domain.Finance;

public partial class AffiliateBankAccount
{
    public int AffiliateId { get; set; }

    public string? BankName { get; set; }

    public string? BankBranch { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? BankAccountType { get; set; }

    public string? BankAccountHolder { get; set; }

    public virtual Affiliate Affiliate { get; set; } = null!;
}
