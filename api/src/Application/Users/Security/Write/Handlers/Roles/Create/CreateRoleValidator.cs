using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using FluentValidation;

namespace DetailingArsenal.Application.Users.Security {
    public class CreateRoleValidator : FluentValidatorAdapter<CreateRoleCommand> {
        public CreateRoleValidator() {
            RuleFor(c => c.Name).MaximumLength(Role.NameMaxLength).WithMessage($"Name must be {Role.NameMaxLength} characters or less.");
        }
    }
}