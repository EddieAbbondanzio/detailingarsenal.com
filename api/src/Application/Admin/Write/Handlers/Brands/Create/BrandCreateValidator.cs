
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Settings;
using FluentValidation;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    public class BrandCreateValidator : FluentValidatorAdapter<BrandCreateCommand> {
        public BrandCreateValidator() {
            RuleFor(cmd => cmd.Name).NotNull().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).MaximumLength(Brand.NameMaxLength).WithMessage($"Name must be {Brand.NameMaxLength} characters or less.");
        }
    }
}