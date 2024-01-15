using AffiliatePMS.Infra.Security.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AffiliatePMS.Infra.Security;

public class AuthService(IConfiguration _configuration, IAuthRepository _authRepository) : IAuthService
{


    /// <summary>
    /// Returns JWT token if login was successful, otherwise null
    /// </summary>
    public async Task<(TokenCredential? tokenCredential, string errorMessage)> LoginAsync(string email, string password)
    {
        // Search user in DB and verify password

        var user = await _authRepository.GetUserAsync(email);

        if (string.IsNullOrEmpty(user?.EncodedPassword) || Argon2.Verify(user.EncodedPassword, password) == false)
        {
            return (null, "Credentials are not valid.");
        }

        // Create JWT token handler and get secret key
        if (user?.Status != UserStatus.Active)
        {
            return (null, "User is not active");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]!);

        // Prepare list of user claims

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,  user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Email, user.Email)
        };

        if (user.AffiliateId.HasValue)
        {
            claims.Add(new Claim("AffiliateId", user.AffiliateId.Value.ToString()));
        }

        // Create token descriptor

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        // Create token and set it to user

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (new TokenCredential(tokenHandler.WriteToken(token)), "login success");
    }

    public async Task<RegisterUserResponse?> RegisterAsync(string name, string email, string password)
    {
        var userDb = await _authRepository.GetUserAsync(email);
        if (userDb != null)
        {
            return RegisterUserResponse.Error($"User {email} already exists");
        }
        var user = await _authRepository.AddUserAsync(name, email, Argon2.Hash(password), affiliateId: null);
        if (user != null)
        {
            return RegisterUserResponse.Success(
                new RegisteredUser(user.Id, user.Email));
        }
        return RegisterUserResponse.Error($"User {email} register unsuccessful");
    }

}
