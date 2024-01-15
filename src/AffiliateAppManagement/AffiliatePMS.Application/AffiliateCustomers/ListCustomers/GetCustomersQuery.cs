using AffiliatePMS.Application.Contracts;
using MediatR;

namespace AffiliatePMS.Application.AffiliateCustomers.ListCustomers
{
    public record GetCustomersQuery : IRequest<List<AffiliateCustomerResponse>>
    {
        public int AffiliateId { get; set; }
    }

    public record GetCustomerByIdQuery : IRequest<AffiliateCustomerResponse>
    {
        public int CustomerId { get; set; }
    }

    public record GetTotalCustomersQuery : IRequest<int>
    {
        public int AffiliateId { get; set; }
    }
}
