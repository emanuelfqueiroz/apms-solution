using AffiliatePMS.Application.Common;
using AffiliatePMS.Application.Contracts;
using FluentValidation;
using MediatR;

namespace AffiliatePMS.Application.Affiliates.Query
{

    public record GetAffiliateByIdQuery(int affiliateId) : IRequest<AffiliateResponse> { }

    public class GetAffiliateQuery : IRequest<List<AffiliateResponse>>
    {
        public int? AffiliateId { get; set; }
    }

    public class GetAffiliateByIdQueryValidator : AbstractValidator<GetAffiliateByIdQuery>
    {
        public GetAffiliateByIdQueryValidator(IIdentifierService identifierService)
        {
            if (identifierService.IsAdmin() is false)
            {
                RuleFor(x => x.affiliateId).Equal(identifierService.AffiliateId!.Value);
            }
        }
    }
}
