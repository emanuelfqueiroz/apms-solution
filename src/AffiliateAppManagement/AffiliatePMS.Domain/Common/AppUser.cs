namespace AffiliatePMS.Domain.Common;

public partial class AppUser
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string EncodedPassword { get; set; } = null!;

    public short Status { get; set; }

    public string Role { get; set; } = null!;
}
