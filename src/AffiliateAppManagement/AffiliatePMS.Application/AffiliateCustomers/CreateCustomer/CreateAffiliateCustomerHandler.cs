using AffiliatePMS.Application.Common;
using AffiliatePMS.Domain.AffiliateCustomers;
using AffiliatePMS.Domain.Common;
using MediatR;

namespace AffiliatePMS.Application.AffiliateCustomers.CreateCustomer
{
    public class CreateAffiliateCustomerHandler(IRepository<AffiliateCustomer> repository, IUnitOfWork uow) : IRequestHandler<CreateAffiliateCustomerCommand, CommandResponse<EntityCreated?>>
    {
        public async Task<CommandResponse<EntityCreated?>> Handle(CreateAffiliateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new AffiliateCustomer()
            {
                AffiliateId = request.AffiliateId,
                FullName = request.FullName,
                Email = request.Email,
                AvgTicket = 0,
                TotalPurchase = 0,
                Gender = ((int?)request.Gender).ToString(),
                BirthDate = request.BirthDate,
            };
            await repository.AddAsync(entity);
            await uow.SaveAsync(cancellationToken);
            return CommandResponse<EntityCreated?>.Success(new EntityCreated() { Id = entity.Id });
        }
    }

}
