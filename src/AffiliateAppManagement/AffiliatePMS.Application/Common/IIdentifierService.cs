namespace AffiliatePMS.Application.Common
{
    public interface IIdentifierService
    {
        int? AffiliateId { get; }
        string GetEmail();
        int GetUserId();
        bool HasUserId();
        bool IsAdmin();
    }
}
