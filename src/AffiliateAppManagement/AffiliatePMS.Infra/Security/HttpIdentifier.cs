using AffiliatePMS.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AffiliatePMS.Infra.Security;

internal class HttpIdentifier(IHttpContextAccessor httpContextAccessor) : IIdentifierService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public int GetUserId() => int.Parse(
            _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

    public string GetEmail() => _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Email)?.Value!;

    public int? AffiliateId
    {
        get
        {
            Claim? id = _httpContextAccessor.HttpContext!.User.FindFirst("affiliateId");
            if (id is null) return null;
            return int.Parse(id.Value);
        }
    }

    public bool HasUserId() => _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier) != null;

    public bool IsAdmin() => _httpContextAccessor.HttpContext!.User.IsInRole(Role.Admin);
}