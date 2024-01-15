using AffiliatePMS.Application.Common;
using AffiliatePMS.Domain.Common;
using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AffiliatePMS.Application.AffiliateCustomers.CreateCustomer
{
    public record CreateAffiliateCustomerCommand : IRequest<CommandResponse<EntityCreated?>>
    {
        public int AffiliateId { get; set; }
        public required string FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Phone { get; init; }
        public DateOnly? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
    public class CreateAffiliateCustomerValidator : AbstractValidator<CreateAffiliateCustomerCommand>
    {
        public CreateAffiliateCustomerValidator()
        {
            RuleFor(x => x.AffiliateId).NotEmpty().WithMessage("AffiliateId is required.");
            RuleFor(x => x.FullName).NotEmpty().Length(10, 100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().Matches(ApplicationValidator.PhoneExpression).WithMessage("A valid Phone Number is required");
            RuleFor(x => x.BirthDate).InclusiveBetween(new DateOnly(1900, 1, 1), DateOnly.FromDateTime(DateTime.Now)).WithMessage("BirthDate must be between 1900-01-01 and today");
            RuleFor(x => x.Gender).IsInEnum().WithMessage("Select a valid gender: " + GenderEnum.Genders.ToText());
        }
    }

}
