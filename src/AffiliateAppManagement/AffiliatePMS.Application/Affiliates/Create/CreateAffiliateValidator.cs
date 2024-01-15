using AffiliatePMS.Application.Common;
using FluentValidation;

namespace AffiliatePMS.Application.Affiliates.Create
{
    public class CreateAffiliateValidator : AbstractValidator<CreateAffiliateCommand>
    {
        public CreateAffiliateValidator()
        {
            RuleFor(x => x.PublicName).NotEmpty().Length(3, 100);
            RuleFor(x => x.FullName).NotEmpty().Length(10, 100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Phone1).NotEmpty().Matches(ApplicationValidator.PhoneExpression).WithMessage("A valid Phone Number is required");
            RuleFor(x => x.SocialMedias).NotEmpty().ForEach(x => x.SetValidator(new CreateSocialMediaValidator()));
        }
        private class CreateSocialMediaValidator : AbstractValidator<CreateAffiliateCommand.CreateAffiliateSocialMedia>
        {
            const int MIN_FOLLOWERS = 10_0000;
            public CreateSocialMediaValidator()
            {
                RuleFor(x => x.Type).NotEmpty().Length(3, 100);
                RuleFor(x => x.Url).NotEmpty().Length(10, 100);
                RuleFor(x => x.Followers).GreaterThanOrEqualTo(MIN_FOLLOWERS).WithMessage($"Social Media must have at least {MIN_FOLLOWERS} followers");
            }
        }
    }
    public class CompleteAffiliateProfileValidator : CreateAffiliateValidator
    {
        public CompleteAffiliateProfileValidator() : base() { }
    }


}
