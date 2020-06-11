using DetailingArsenal.Domain;
using FluentValidation;

namespace DetailingArsenal.Application {
    public class CreateRoleValidator : FluentValidatorAdapter<CreateRoleCommand> {
        public CreateRoleValidator() {
            RuleFor(c => c.Name).MaximumLength(Role.NameMaxLength).WithMessage($"Name must be {Role.NameMaxLength} characters or less.");
        }
    }
}