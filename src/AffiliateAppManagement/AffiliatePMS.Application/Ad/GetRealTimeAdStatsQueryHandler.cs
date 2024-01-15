using AffiliatePMS.Application.Contracts;
using MediatR;

namespace AffiliatePMS.Application.Ad
{
    public class GetRealTimeAdStatsQueryHandler(IRealTimeStatisRepository repository) : IRequestHandler<AdStatsQuery, List<AdRealTimeStats>>
    {
        public async Task<List<AdRealTimeStats>> Handle(AdStatsQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(await repository.GetRealTimeStatsAsync(request.AffiliateId));
        }
    }
}
