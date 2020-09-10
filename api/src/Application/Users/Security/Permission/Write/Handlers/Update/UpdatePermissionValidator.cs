using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using FluentValidation;

namespace DetailingArsenal.Application.Users.Security {
    public class UpdatePermissionValidator : FluentValidatorAdapter<UpdatePermissionCommand> {
        public UpdatePermissionValidator() {
            RuleFor(c => c.Action).MaximumLength(Permission.ActionMaxLength).WithMessage($"Name must be {Permission.ActionMaxLength} characters or less.");
            RuleFor(c => c.Scope).MaximumLength(Permission.ScopeMaxLength).WithMessage($"Name must be {Permission.ScopeMaxLength} characters or less.");
        }
    }
}