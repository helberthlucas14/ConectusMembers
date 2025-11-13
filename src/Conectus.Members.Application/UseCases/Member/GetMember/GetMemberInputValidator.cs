using FluentValidation;

namespace Conectus.Members.Application.UseCases.Member.GetMember
{
    public class GetMemberInputValidator
        : AbstractValidator<GetMemberInput>
    {
        public GetMemberInputValidator()
        {
           RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
