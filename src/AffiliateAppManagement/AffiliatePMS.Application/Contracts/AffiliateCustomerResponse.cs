using AffiliatePMS.Domain.Common;

namespace AffiliatePMS.Application.Contracts
{
    public record AffiliateCustomerResponse
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public Gender? GenderId { get; set; }
        public string? GenderName => GenderId?.ToString();
        public DateOnly? BirthDate { get; set; }
        public decimal? AvgTicket { get; set; }
        public decimal? TotalPurchase { get; set; }
    }


}
