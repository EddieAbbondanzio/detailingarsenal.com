using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using FluentValidation;

namespace DetailingArsenal.Application.Security {
    public class UpdateRoleValidator : FluentValidatorAdapter<UpdateRoleCommand> {
        public UpdateRoleValidator() {
            RuleFor(c => c.Name).MaximumLength(Role.NameMaxLength).WithMessage($"Name must be {Role.NameMaxLength} characters or less.");
        }
    }
}