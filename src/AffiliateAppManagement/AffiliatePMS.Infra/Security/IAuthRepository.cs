using AffiliatePMS.Infra.Security.Models;

namespace AffiliatePMS.Infra.Security;

public interface IAuthRepository
{
    Task<UserAuth?> GetUserAsync(string email);
    Task<UserAuth?> AddUserAsync(string name, string email, string encodedPassword, int? affiliateId);
}
