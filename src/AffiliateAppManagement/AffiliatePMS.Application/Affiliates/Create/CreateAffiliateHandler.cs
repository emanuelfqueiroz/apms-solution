using AffiliatePMS.Application.Common;
using AffiliatePMS.Domain.Affiliates;
using AffiliatePMS.Domain.Affiliates.Events;
using MediatR;

namespace AffiliatePMS.Application.Affiliates.Create
{
    public class CreateAffiliateHandler :
        IRequestHandler<CreateAffiliateCommand, CommandResponse<EntityCreated>>,
        IRequestHandler<CreateAffiliateProfileCommand, CommandResponse<EntityCreated>>
    {
        private readonly IAffiliateRepository affiliateRepository;
        private readonly IUnitOfWork uow;
        private readonly IIdentifierService identifierService;
        private readonly INotificationPublisher eventPublisher;

        public CreateAffiliateHandler(IAffiliateRepository affiliateRepository, IUnitOfWork uow, IIdentifierService identifierService)
        {
            this.affiliateRepository = affiliateRepository;
            this.uow = uow;
            this.identifierService = identifierService;
        }

        public async Task<CommandResponse<EntityCreated>> Handle(CreateAffiliateCommand command, CancellationToken cancellationToken)
        {

            bool isEmailUsed = await affiliateRepository.IsEmailUsedAsync(command.Email);

            if (isEmailUsed)
            {
                return CommandResponse<EntityCreated>.Error("Profile already created");
            }

            var entity = new Affiliate()
            {
                PublicName = command.PublicName,
                AffiliateSocialMedia = command.SocialMedias.Select(s => new AffiliateSocialMedia()
                {
                    Type = s.Type,
                    Url = s.Url,
                    Followers = s.Followers
                }).ToArray(),
                AffiliateDetail = new AffiliateDetail()
                {
                    FullName = command.FullName,
                    Email = command.Email,
                    Phone1 = command.Phone1,
                    Phone2 = command.Phone2
                },
                UserCreatedId = identifierService.GetUserId(),
            };
            await affiliateRepository.AddAsync(entity);
            await uow.SaveAsync(cancellationToken);
            return CommandResponse<EntityCreated>.Success(new EntityCreated() { Id = entity.Id });
        }

        /// <summary>
        /// After Login,  affiliate user create his profile
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CommandResponse<EntityCreated>> Handle(CreateAffiliateProfileCommand command, CancellationToken cancellationToken)
        {
            if (command.Email != identifierService.GetEmail())
            {
                return CommandResponse<EntityCreated>.Error("You must use the same login email.");
            }

            var response = await this.Handle((CreateAffiliateCommand)command, cancellationToken);
            if (response.IsSuccess)
            {
                var @profileCreated = new AffiliateProfileCreatedEvent();

                // ASSOCIATE the profile with the user
                // await eventPublisher.Publish(@profileCreated);
            }
            return response;
        }
    }
}
