using MediatR;

namespace AffiliatePMS.Application.Affiliates.Query
{
    public class GetAffiliateHeadQuery : IRequest<int>
    {
        public int? AffiliateId { get; set; }
    }
}
