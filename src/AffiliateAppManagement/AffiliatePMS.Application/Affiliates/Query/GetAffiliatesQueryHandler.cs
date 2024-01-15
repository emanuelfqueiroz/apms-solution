using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Domain.Affiliates;
using Mapster;
using MediatR;

namespace AffiliatePMS.Application.Affiliates.Query
{
    public class GetAffiliatesQueryHandler(IAffiliateRepository repository)
        : IRequestHandler<GetAffiliateQuery, List<AffiliateResponse>>, IRequestHandler<GetAffiliateHeadQuery, int>, IRequestHandler<GetAffiliateByIdQuery, AffiliateResponse>

    {
        public async Task<List<AffiliateResponse>> Handle(GetAffiliateQuery request, CancellationToken cancellationToken)
        {

            var data = await repository.GetAllAsync();
            return data.Adapt<List<AffiliateResponse>>();
        }

        public async Task<int> Handle(GetAffiliateHeadQuery request, CancellationToken cancellationToken)
        {
            return await repository.TotalAsync();
        }

        public async Task<AffiliateResponse> Handle(GetAffiliateByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.GetAsync(request.affiliateId);
            return result.Adapt<AffiliateResponse>();
        }
    }


}
