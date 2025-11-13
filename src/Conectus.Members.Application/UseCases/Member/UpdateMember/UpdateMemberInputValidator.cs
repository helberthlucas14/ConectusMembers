using FluentValidation;

namespace Conectus.Members.Application.UseCases.Member.UpdateMember
{
    public class UpdateMemberInputValidator 
        : AbstractValidator<UpdateMemberInput>
    {
        public UpdateMemberInputValidator() => RuleFor(x => x.Id).NotEmpty();
    }
}
