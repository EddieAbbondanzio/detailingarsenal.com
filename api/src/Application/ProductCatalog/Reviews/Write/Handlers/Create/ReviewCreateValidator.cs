
using System;
using DetailingArsenal.Application.ProductCatalog;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Settings;
using FluentValidation;

namespace DetailingArsenal.Application.ProductCatalog {
    public class ReviewCreateValidator : FluentValidatorAdapter<ReviewCreateCommand> {
        public ReviewCreateValidator() {
            RuleFor(c => c.PadId).NotEqual(Guid.Empty).WithMessage("Pad id is required.");
            RuleFor(c => c.Stars).InclusiveBetween(1, 5).WithMessage("Stars must be between 1 to 5.");

            When(c => c.Cut != null, () => {
                RuleFor(c => c.Cut).InclusiveBetween(0, 10).WithMessage("Cut must be between 0 to 10.");
            });

            When(c => c.Finish != null, () => {
                RuleFor(c => c.Finish).InclusiveBetween(0, 10).WithMessage("Finish must be between 0 to 10.");
            });

            RuleFor(c => c.Title).NotNull().WithMessage("Title is required.");
            RuleFor(c => c.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(c => c.Title).MaximumLength(64).WithMessage("Title must be 64 characters or less.");

            RuleFor(c => c.Body).NotNull().WithMessage("Body is required.");
            RuleFor(c => c.Body).NotEmpty().WithMessage("Body is required.");
            RuleFor(c => c.Title).MaximumLength(10_000).WithMessage("Body must be 10,000 characters or less.");
        }
    }
}