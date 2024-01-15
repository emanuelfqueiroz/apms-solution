using System.Text.Json.Serialization;

namespace AffiliatePMS.Infra.Security.Models;

public class UserAuth
{
    public UserAuth(int id, string name, string email, string encodedPassword, UserStatus status, string role, int? affiliateId)
    {
        Id = id;
        Name = name;
        Email = email;
        EncodedPassword = encodedPassword;
        Status = status;
        Role = role;
        AffiliateId = affiliateId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public UserStatus Status { get; set; }

    public int? AffiliateId { get; set; }

    [JsonIgnore]
    public string EncodedPassword { get; set; }
}