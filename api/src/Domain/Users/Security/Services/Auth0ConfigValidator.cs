using FluentValidation;

namespace DetailingArsenal.Domain.Users {
    public class Auth0ConfigValidator : FluentValidatorAdapter<Auth0Config> {
        public Auth0ConfigValidator() {
            RuleFor(c => c.Domain).NotEmpty().WithMessage("Auth0 domain is required.");
            RuleFor(c => c.Identifier).NotEmpty().WithMessage("Auth0 identifier is required.");
            RuleFor(c => c.ClientId).NotEmpty().WithMessage("Auth0 client id is required.");
            RuleFor(c => c.ClientSecret).NotEmpty().WithMessage("Auth0 client secret is required.");
            RuleFor(c => c.Connection).NotEmpty().WithMessage("Auth0 database connection is required.");
        }
    }
}