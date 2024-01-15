using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Domain.AffiliateCustomers;
using AffiliatePMS.Domain.Common;
using Mapster;
using MediatR;
using System.Linq.Expressions;

namespace AffiliatePMS.Application.AffiliateCustomers.ListCustomers
{
    public class GetCustomersQueryHandler(IRepository<AffiliateCustomer> repository) :
        IRequestHandler<GetCustomersQuery, List<AffiliateCustomerResponse>>,
        IRequestHandler<GetCustomerByIdQuery, AffiliateCustomerResponse>,
        IRequestHandler<GetTotalCustomersQuery, int>
    {
        public async Task<List<AffiliateCustomerResponse>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<AffiliateCustomer, bool>> predicate = p => p.AffiliateId == request.AffiliateId;
            var data = await repository.GetAllAsync(predicate);
            return data.Adapt<List<AffiliateCustomerResponse>>();
        }

        public async Task<AffiliateCustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetAsync(request.CustomerId);
            return entity.Adapt<AffiliateCustomerResponse>();
        }

        public async Task<int> Handle(GetTotalCustomersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<AffiliateCustomer, bool>> predicate = p => p.AffiliateId == request.AffiliateId;
            return await repository.CountAssync(predicate);
        }
    }

}
