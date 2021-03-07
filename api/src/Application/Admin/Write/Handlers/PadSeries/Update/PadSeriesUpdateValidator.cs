using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using FluentValidation;

namespace DetailingArsenal.Application.Admin.ProductCatalog {
    [DependencyInjection]
    public class PadSeriesUpdateValidator : FluentValidatorAdapter<PadSeriesUpdateCommand> {
        public PadSeriesUpdateValidator() {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(c => c.Name).MaximumLength(32).WithMessage("Name must be 32 characters or less.");

            RuleForEach(c => c.Pads).ChildRules(pads => {
                pads.RuleFor(p => p.Name).NotEmpty().WithMessage("Pad name is required.");
                pads.RuleFor(p => p.Name).MaximumLength(32).WithMessage("Pad name must be 32 characters or less.");

                pads.RuleForEach(p => p.Options).ChildRules(options => {
                    options.RuleForEach(o => o.PartNumbers).ChildRules(partNumbers => {
                        partNumbers.RuleFor(pn => pn.Value).MaximumLength(64).WithMessage("Part number must be 64 characters or less.");
                        partNumbers.RuleFor(pn => pn.Notes).MaximumLength(128).WithMessage("Notes must be 128 characters or less.");
                    });
                });
            });
        }
    }
}