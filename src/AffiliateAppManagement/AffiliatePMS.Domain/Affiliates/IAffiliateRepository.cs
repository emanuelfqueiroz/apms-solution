using AffiliatePMS.Domain.Common;

namespace AffiliatePMS.Domain.Affiliates;

public interface IAffiliateRepository : IRepository<Affiliate>
{
    Task<bool> IsEmailUsedAsync(string email);
    Task<int> TotalAsync();
}

