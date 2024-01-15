using AffiliatePMS.Application.Contracts;
using MediatR;

namespace AffiliatePMS.Application.Ad
{
    public class AdStatsQuery : IRequest<List<AdRealTimeStats>>
    {
        public int AffiliateId { get; set; }
    }
}
