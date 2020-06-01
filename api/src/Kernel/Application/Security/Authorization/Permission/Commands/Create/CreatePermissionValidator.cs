using FluentValidation;

public class CreatePermissionValidator : FluentValidatorAdapter<CreatePermissionCommand> {
    public CreatePermissionValidator() {
        RuleFor(c => c.Action).MaximumLength(Permission.ActionMaxLength).WithMessage($"Name must be {Permission.ActionMaxLength} characters or less.");
        RuleFor(c => c.Scope).MaximumLength(Permission.ScopeMaxLength).WithMessage($"Name must be {Permission.ScopeMaxLength} characters or less.");
    }
}