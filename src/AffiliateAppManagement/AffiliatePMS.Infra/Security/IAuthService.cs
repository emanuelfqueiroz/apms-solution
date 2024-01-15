using AffiliatePMS.Infra.Security.Models;

namespace AffiliatePMS.Infra.Security;

public interface IAuthService
{
    /// <summary>
    /// Returns JWT token if login was successful, otherwise null
    /// </summary>
    Task<(TokenCredential? tokenCredential, string errorMessage)> LoginAsync(string email, string password);
    /// <summary>
    /// Returns userId if registration was successful, otherwise null
    /// </summary>
    Task<RegisterUserResponse?> RegisterAsync(string Name, string Email, string Password);
}
