using AffiliatePMS.Application.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AffiliatePMS.Application.Affiliates.Create
{

    public record CreateAffiliateCommand : IRequest<CommandResponse<EntityCreated>>
    {
        public required string PublicName { get; set; }
        public required string FullName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public string? Phone1 { get; init; }
        [Phone]
        public string? Phone2 { get; init; }

        public required CreateAffiliateSocialMedia[] SocialMedias { get; set; }

        public record CreateAffiliateSocialMedia
        {
            public required string Url { get; set; }
            public required string Type { get; set; }
            public required int Followers { get; set; }
        }
    }

    public record CreateAffiliateProfileCommand : CreateAffiliateCommand, IRequest<CommandResponse<EntityCreated>>
    {

    }


}
